using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static void ConfigureModelBindingMessages(this IMvcBuilder mvcBuilder, string resourceName = null, string resourceLocation = null)
        {
            mvcBuilder.Services.Configure<MvcOptions>(opt =>
            {
                var stringLocalizerFactory = mvcBuilder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();

                // By default, the Resx file name is ModelBindingDefaultMessages.resx:
                resourceName ??= "ModelBindingDefaultMessages";
                // By default, resources live in same assembly that the Startup class does:
                resourceLocation ??= Assembly.GetExecutingAssembly().GetName().Name;

                var loc = stringLocalizerFactory.Create(resourceName, resourceLocation);

                opt.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
                    prop => loc["MissingBindRequired", prop]);
                opt.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                    () => loc["MissingKeyOrValue"]);
                opt.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(
                    () => loc["MissingRequestBodyRequired"]);
                opt.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    prop => loc["ValueMustNotBeNull"]);
                opt.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                    (value, prop) => loc["AttemptedValueIsInvalid", value, prop]);
                opt.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(
                    value => loc["NonPropertyAttemptedValue", value]);
                opt.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(
                    prop => loc["UnknownValueIsInvalid", prop]);
                opt.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(
                    () => loc["NonPropertyUnknownValueIsInvalid"]);
                opt.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                    value => loc["ValueIsInvalid", value]);
                opt.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                    prop => loc["ValueMustBeANumber", prop]);
                opt.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(
                    () => loc["NonPropertyValueMustBeNumber"]);
            });
        }
    }
}