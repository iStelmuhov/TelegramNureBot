using System;
using System.Collections.Generic;
using System.Text;
using Exceptions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Dependencies.Aspects
{
    class ExceptionInterceptionBehavior : IInterceptionBehavior
    {
        private class ValidationTranslateHandler : IExceptionHandler
        {
            public Exception HandleException(Exception exception, Guid handlingInstanceId)
            {
                ArgumentValidationException ae = exception as ArgumentValidationException;
                if (ae == null)
                    return exception;

                StringBuilder builder = new StringBuilder();
                builder.Append("Data validation error.\nID = ");
                builder.Append(handlingInstanceId);
                builder.AppendLine();
                builder.Append(exception.Message);
                builder.AppendLine();
                foreach (var validationResult in ae.ValidationResults)
                    builder.Append(validationResult.Message);

                return new ServiceValidationException(builder.ToString());
            }
        }

        public ExceptionInterceptionBehavior()
        {
            var exceptionShielding = new List<ExceptionPolicyEntry>
            {
                    new ExceptionPolicyEntry(
                        typeof( ArgumentValidationException ),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[]
                        {
                            new ValidationTranslateHandler()
                        }
                    ),

                    new ExceptionPolicyEntry(
                        typeof( DomainLogicException ),
                        PostHandlingAction.NotifyRethrow,
                        new IExceptionHandler[]
                        {
                            // No handlers
                        }
                    ),

                    new ExceptionPolicyEntry(
                        typeof( Exception ),
                        PostHandlingAction.ThrowNewException,
                        new IExceptionHandler[]
                        {
                            new ReplaceHandler(
                                "Application Error. Please contact your administrator.\nError ID: {handlingInstanceID}",
                                typeof( ApplicationFatalException )
                            )
                        }
                    )
            };

            var policies = new List<ExceptionPolicyDefinition>();
            policies.Add(new ExceptionPolicyDefinition(ExceptionShieldingPolicy, exceptionShielding));

            exceptionManager = new ExceptionManager(policies);
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn result = null;
            result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                Exception outException = null;
                if (exceptionManager.HandleException(result.Exception, ExceptionShieldingPolicy, out outException))
                    throw outException ?? result.Exception;
            }

            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute => true;

        private ExceptionManager exceptionManager;

        private static readonly string ExceptionShieldingPolicy = "ExceptionShielding";
    }
}