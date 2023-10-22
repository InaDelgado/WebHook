﻿using WebHook.Models;

namespace WebHook.Interfaces
{
    public interface IReceiveWebhook
    {
        Task<string> SendRequest(string user, string repository, GitHubAccessToken accessToken);
    }
}