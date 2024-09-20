using Google.Cloud.TextToSpeech.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoMOAB.Controller
{
    public class TextSpeechController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        public TextSpeechController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpPost]
        public IActionResult ConvertTextToSpeech([FromForm] string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("O campo de texto não pode estar vazio.");
            }

            // Configurar o caminho para o arquivo de credenciais JSON
            string NomeArquivo = "google-credentials.json";
            string CaminhoArquivo = System.IO.Path.Combine(_env.WebRootPath, "credentials", NomeArquivo);

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CaminhoArquivo);

            // Criar uma instância do cliente do Google Text-to-Speech
            var client = TextToSpeechClient.Create();

            // Configurar a requisição de síntese de fala
            var synthesisInput = new SynthesisInput
            {
                Text = text
            };

            // Definir a voz e o idioma (ex: português do Brasil)
            var voice = new VoiceSelectionParams
            {
                LanguageCode = "pt-BR",
                SsmlGender = SsmlVoiceGender.Female
            };

            // Definir o formato de saída do áudio (ex: MP3)
            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            // Fazer a requisição para sintetizar o texto em fala
            var response = client.SynthesizeSpeech(synthesisInput, voice, audioConfig);

            // Retornar o áudio como um arquivo MP3
            return File(response.AudioContent.ToByteArray(), "audio/mpeg", "output.mp3");
        }

        // Classe para receber o texto da requisição
        public class TextInput
        {
            public string? Text { get; set; }
        }
    }


}
