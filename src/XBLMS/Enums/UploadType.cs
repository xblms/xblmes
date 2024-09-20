using Datory.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace XBLMS.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UploadType
    {
        [DataEnum(DisplayName = "Image")]
        Image,
        [DataEnum(DisplayName = "Audio")]
        Audio,
        [DataEnum(DisplayName = "Video")]
        Video,
        [DataEnum(DisplayName = "File")]
        File,
        [DataEnum(DisplayName = "Special")]
        Special
    }
    public enum UploadManageType
    {
        [DataEnum(DisplayName = "AdminAvatar")]
        AdminAvatar,
        [DataEnum(DisplayName = "UserAvatar")]
        UserAvatar,
    }
}
