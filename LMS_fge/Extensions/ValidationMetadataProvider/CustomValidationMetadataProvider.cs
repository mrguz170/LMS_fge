using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace LMS_fge.Extensions.ValidationMetadataProvider
{
    public class CustomValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly ResourceManager _resourceManager;
        private readonly Type _resourceType;

        // Original validation messages:
        // https://github.com/dotnet/corefx/blob/master/src/System.ComponentModel.Annotations/src/Resources/Strings.resx

        public CustomValidationMetadataProvider(Type type)

        {
            _resourceType = type;
            _resourceManager = new ResourceManager(type.FullName, type.GetTypeInfo().Assembly);
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context.Key.ModelType.GetTypeInfo().IsValueType
                && Nullable.GetUnderlyingType(context.Key.ModelType.GetType()) != null
                && !context.ValidationMetadata.ValidatorMetadata.OfType<RequiredAttribute>().Any())
            {
                context.ValidationMetadata.ValidatorMetadata.Add(new RequiredAttribute());
            }

            foreach (var validationAttribute in context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>())
            {
                if (validationAttribute.ErrorMessageResourceName == null && validationAttribute.ErrorMessageResourceType == null)
                {
                    // By convention, the resource key will coincide with the attribute
                    // name, removing the suffix "Attribute" when needed
                    var resourceKey = validationAttribute.GetType().Name;
                    if (resourceKey.EndsWith("Attribute"))
                    {
                        resourceKey = resourceKey[0..^9];
                    }

                    // Patch the "StringLength with minimum value" case
                    if (validationAttribute is StringLengthAttribute stringLength && stringLength.MinimumLength > 0)
                    {
                        resourceKey = "StringLengthIncludingMinimum";
                    }

                    // Setup the message if the key exists
                    if (_resourceManager.GetString(resourceKey) != null)
                    {
                        validationAttribute.ErrorMessage = null;
                        validationAttribute.ErrorMessageResourceType = _resourceType;
                        validationAttribute.ErrorMessageResourceName = resourceKey;
                    }
                }
            }
        }
    }
}
