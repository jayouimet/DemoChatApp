using ChatApp.Client.Services;
using ChatApp.Common.Dtos.Message;
using Fluxor;

namespace ChatApp.Client.Stores;

[FeatureState]
public class MessageListState
{
    public IReadOnlyList<MessageDto> Data { get; private set; }
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    public MessageListState()
    {
        Data = new List<MessageDto>();
        IsLoading = false;
        ErrorMessage = null;
    }

    public MessageListState(List<MessageDto> data, bool loading, string? errorMessage = null)
    {
        Data = data;
        IsLoading = loading;
        ErrorMessage = errorMessage;
    }
}

public class FetchMessageListAction
{
    public long UserId { get; private set; }
    
    public FetchMessageListAction(long userId)
    {
        UserId = userId;
    }
}

public class FetchMessageListSuccessAction
{
    public IReadOnlyList<MessageDto> Data { get; private set; }

    public FetchMessageListSuccessAction(List<MessageDto> data)
    {
        Data = data;
    }
}

public class FetchMessageListFailureAction
{
    public string ErrorMessage { get; private set; }

    public FetchMessageListFailureAction(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
};

public static class MessageListReducers
{
    [ReducerMethod]
    public static MessageListState ReduceFetchMessageListAction(MessageListState state, FetchMessageListAction action) =>
        new(new List<MessageDto>(), true, null);
    
    [ReducerMethod]
    public static MessageListState ReduceFetchMessageListSuccessAction(MessageListState state, FetchMessageListSuccessAction action) =>
        new(action.Data.ToList(), false, null);

    [ReducerMethod]
    public static MessageListState ReduceFetchMessageListFailureAction(MessageListState state, FetchMessageListFailureAction action) =>
        new(new List<MessageDto>(), true, action.ErrorMessage);
}

public class FetchMessageListEffects
{
    private readonly ApiServices _apiServices;

    public FetchMessageListEffects(ApiServices apiServices)
    {
        _apiServices = apiServices;
    }
    
    [EffectMethod]
    public async Task HandleFetchMessageListAction(FetchMessageListAction action, IDispatcher dispatcher)
    {
        try
        {
            var result = await _apiServices.Message.GetMessages(action.UserId);

            if (result.Success)
            {
                dispatcher.Dispatch(new FetchMessageListSuccessAction(new List<MessageDto>(result.Data!)));
            }
            else 
            {
                dispatcher.Dispatch(new FetchMessageListFailureAction(result.ErrorKey!));
            }
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new FetchMessageListFailureAction(ex.Message));
        }
    }
}