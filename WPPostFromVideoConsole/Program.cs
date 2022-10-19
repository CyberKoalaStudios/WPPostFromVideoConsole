using WPPostFromVideoConsole;

DotNetEnv.Env.TraversePath().Load();

var secrets = new string[2] {"client_secrets.json", ""};

MyUploads.GetUploads(secrets);