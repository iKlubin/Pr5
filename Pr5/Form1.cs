using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Drawing;

namespace Pr5
{
    public partial class Form1 : Form
    {
        private DiceGame diceGame = new DiceGame();

        private int player1Score = 0;
        private int player2Score = 0;
        private int count = 0;

        public Form1()
        {
            InitializeComponent();

            label3.Text += player1Score;
            label4.Text += player2Score;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            label5.Text = "Ход: " + count.ToString();
        }

        private async Task RollDiceAndUpdateUI(int playerNumber, Label diceLabel, Label scoreLabel)
        {
            try
            {
                int diceValues = await diceGame.RollDiceAsync(6);

                if (playerNumber == 1)
                {
                    int playerScore = diceValues + int.Parse(label3.Text.Split(' ')[1]);
                    player1Score = playerScore;
                    diceLabel.Text = $"?";
                    pictureBox1.Image = Image.FromFile("Resources\\1.png");
                    await Task.Delay(50);
                    pictureBox1.Image = Image.FromFile("Resources\\6.png");
                    await Task.Delay(50);
                    pictureBox1.Image = Image.FromFile("Resources\\4.png");
                    await Task.Delay(50);
                    pictureBox1.Image = Image.FromFile("Resources\\3.png");
                    await Task.Delay(50);
                    pictureBox1.Image = Image.FromFile("Resources\\2.png");
                    await Task.Delay(50);
                    pictureBox1.Image = Image.FromFile("Resources\\1.png");
                    await Task.Delay(50);
                    pictureBox1.Image = Image.FromFile("Resources\\5.png");
                    await Task.Delay(50);
                    scoreLabel.Text = $"Игрок1: {player1Score}";
                    diceLabel.Text = $"{diceValues}";
                    switch (diceValues)
                    {
                        case 1:
                            pictureBox1.Image = Image.FromFile("Resources\\1.png");
                            break;
                        case 2:
                            pictureBox1.Image = Image.FromFile("Resources\\2.png");
                            break;
                        case 3:
                            pictureBox1.Image = Image.FromFile("Resources\\3.png");
                            break;
                        case 4:
                            pictureBox1.Image = Image.FromFile("Resources\\4.png");
                            break;
                        case 5:
                            pictureBox1.Image = Image.FromFile("Resources\\5.png");
                            break;
                        case 6:
                            pictureBox1.Image = Image.FromFile("Resources\\6.png");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    int playerScore = diceValues + int.Parse(label4.Text.Split(' ')[1]);
                    player2Score = playerScore;
                    diceLabel.Text = $"?";
                    pictureBox2.Image = Image.FromFile("Resources\\1.png");
                    await Task.Delay(50);
                    pictureBox2.Image = Image.FromFile("Resources\\6.png");
                    await Task.Delay(50);
                    pictureBox2.Image = Image.FromFile("Resources\\4.png");
                    await Task.Delay(50);
                    pictureBox2.Image = Image.FromFile("Resources\\3.png");
                    await Task.Delay(50);
                    pictureBox2.Image = Image.FromFile("Resources\\2.png");
                    await Task.Delay(50);
                    pictureBox2.Image = Image.FromFile("Resources\\1.png");
                    await Task.Delay(50);
                    pictureBox2.Image = Image.FromFile("Resources\\5.png");
                    await Task.Delay(50);
                    scoreLabel.Text = $"Игрок2: {player2Score}";
                    diceLabel.Text = $"{diceValues}";
                    switch (diceValues)
                    {
                        case 1:
                            pictureBox2.Image = Image.FromFile("Resources\\1.png");
                            break;
                        case 2:
                            pictureBox2.Image = Image.FromFile("Resources\\2.png");
                            break;
                        case 3:
                            pictureBox2.Image = Image.FromFile("Resources\\3.png");
                            break;
                        case 4:
                            pictureBox2.Image = Image.FromFile("Resources\\4.png");
                            break;
                        case 5:
                            pictureBox2.Image = Image.FromFile("Resources\\5.png");
                            break;
                        case 6:
                            pictureBox2.Image = Image.FromFile("Resources\\6.png");
                            break;
                        default:
                            break;
                    }
                }

                if (count >= 5)
                {
                    string t = "Ничья!";
                    if (player1Score > player2Score)
                    {
                        t = "Игрок1 выйграл!";
                    }
                    else
                    {
                        t = "Игрок2 выйграл!";
                    }
                    MessageBox.Show($"Конец игры: {t}", "Конец", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await RollDiceAndUpdateUI(1, label1, label3);
            button1.Enabled = false;
            button2.Enabled = true;
            count++;
            label5.Text = "Ход: " + count.ToString();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await RollDiceAndUpdateUI(2, label2, label4);
            button2.Enabled = false;
            button1.Enabled = true;
            count++;
            label5.Text = "Ход: " + count.ToString();
        }
    }

    public class DiceGame
    {
        private RandomOrgApi randomOrgApi = new RandomOrgApi();

        public async Task<int> RollDiceAsync(int maxValue)
        {
            return await randomOrgApi.RollDiceAsync(maxValue);
        }
    }

    public class RandomOrgApi
    {
        private const string ApiUrl = "https://api.random.org/json-rpc/2/invoke";
        private const string ApiKey = "9323b628-0c5e-4056-970e-a2772571423e";

        public async Task<int> RollDiceAsync(int maxValue)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestData = new
                {
                    jsonrpc = "2.0",
                    method = "generateIntegers",
                    @params = new
                    {
                        apiKey = ApiKey,
                        n = 1, // Изменен параметр на 1 для получения одного числа
                        min = 1,
                        max = maxValue,
                        replacement = true
                    },
                    id = Guid.NewGuid().ToString()
                };

                string jsonRequest = JsonConvert.SerializeObject(requestData);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(ApiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

                        if (responseObject != null && responseObject.result != null && responseObject.result.random != null && responseObject.result.random.data != null)
                        {
                            JArray dataArray = responseObject.result.random.data;

                            // Преобразуйте JArray в массив чисел.
                            int diceValue = dataArray[0].Value<int>(); // Получаем первый элемент массива

                            // Возвращаем число, представляющее результат броска костей.
                            return diceValue;
                        }
                        else
                        {
                            throw new HttpRequestException("Failed to parse random numbers from Random.org response");
                        }
                    }
                    else
                    {
                        throw new HttpRequestException("Failed to retrieve random numbers from Random.org");
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new HttpRequestException($"HTTP request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new HttpRequestException($"Error: {ex.Message}");
                }
            }
        }
    }
}
