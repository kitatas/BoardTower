namespace BoardTower.Common.Application
{
    public static class EnumExtension
    {
        public static string ToURL(this UrlType self)
        {
            return self switch
            {
                UrlType.Apps => UrlConfig.APP_URL,
                UrlType.DeveloperApps => UrlConfig.DEVELOPER_APP_URL,
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_URL),
            };
        }
    }
}