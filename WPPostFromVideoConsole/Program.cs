using WPPostFromVideoConsole;

DotNetEnv.Env.TraversePath().Load();

var secrets = new string[2] {DotNetEnv.Env.GetString("CLIENT_SECRETS_FILE"), ""};

MyUploads.GetUploads(secrets);