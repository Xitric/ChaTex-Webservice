using Business.Models;

namespace WebAPI.Models.Mappers
{
    class MessageMapper
    {
        public static GetMessageDTO MapMessageToGetMessageDTO(MessageModel messageModel, int callerId)
        {
            if (messageModel == null) return null;
            return new GetMessageDTO(messageModel.Id, messageModel.CreationTime, messageModel.Content, messageModel.DeletionTime, messageModel.LastEdited, UserMapper.MapUserToUserDTO(messageModel.Author, callerId));
        }

        public static MessageEventDTO MapMessageEventToMessageEventDTO(MessageEventModel messageEventModel, int callerId)
        {
            if (messageEventModel == null) return null;
            return new MessageEventDTO(messageEventModel.Type.ToString(), MapMessageToGetMessageDTO(messageEventModel.Message, callerId));
        }
    }
}
