using DotNetEnv;
using WPPostFromVideoConsole;

Env.TraversePath().Load();


var secrets = new[] { Env.GetString("CLIENT_SECRETS_FILE") };

var publisher = new Publisher();
await publisher.StatusChecker();

MyUploads.GetUploads(secrets);
