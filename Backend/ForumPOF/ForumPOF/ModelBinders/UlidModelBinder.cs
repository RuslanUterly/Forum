using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ForumPOF.ModelBinders;

public class UlidModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // Логика биндинга модели
        var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
        // Преобразование value в нужный тип данных

        if (!Ulid.TryParse(value, out Ulid ulid))
            bindingContext.Result = ModelBindingResult.Failed();
        
        bindingContext.Result = ModelBindingResult.Success(ulid);

        return Task.CompletedTask;
    }
}
