using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SudokuSolver
{
    static class RandomBoard
    {
        public class Grid
        {
            [JsonPropertyName("value")]
            public int[][] Value { get; set; }
        }

        public class NewBoard
        {
            [JsonPropertyName("grids")]
            public Grid[] Grids { get; set; }
        }

        private class JsonResponse
        {
            [JsonPropertyName("newboard")]
            public NewBoard NewBoard { get; set; }
        }


        public static int[][] Get()
        {
            const string URL = "https://sudoku-api.vercel.app/api/dosuku?query={newboard(limit:1){grids{value}}}";
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = client.GetAsync(URL).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                string s = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(s);

                JsonResponse json = JsonSerializer.Deserialize<JsonResponse>(s);

                NewBoard b = json.NewBoard;

                Grid[] gs = b.Grids;

                Grid g = gs[0];

                int[][] a = g.Value;




                return json.NewBoard.Grids[0].Value;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to get a random board:\n{e}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return new int[9][];
        }
    }
}
