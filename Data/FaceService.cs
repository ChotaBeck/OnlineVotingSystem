using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace OnlineVotingSystem.Data
{
    public class FaceService
    {
        private static string apiKey;
        private static string endpoint;

    public static FaceClient Authenticate(IConfiguration configuration)
    {
        apiKey = configuration["FaceApiKey"];
        endpoint = configuration["FaceApiEndpoint"];

        return new FaceClient(new ApiKeyServiceClientCredentials(apiKey)) { Endpoint = endpoint };
    }
    }
}