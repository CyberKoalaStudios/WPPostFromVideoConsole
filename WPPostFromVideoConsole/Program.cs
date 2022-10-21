using DotNetEnv;
using WPPostFromVideoConsole;

Env.TraversePath().Load();


var secrets = new string[1] { Env.GetString("CLIENT_SECRETS_FILE") };
MyUploads.GetUploads(secrets);