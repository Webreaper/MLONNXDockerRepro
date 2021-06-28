using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace Yolov5Net.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = new DirectoryInfo("/Users/markotway/LocalCloud/Development/Damselfly/Damselfly.Web/config/thumbs");
            var scorer = new YoloScorer<YoloCocoModel>();
            var files = dir.GetFiles("*_m.jpg", SearchOption.AllDirectories)
                           .OrderBy(x => x.Name)
                           .ToList();

            foreach (var file in files)
                ProcessImage(file, scorer);
        }

        private static void ProcessImage( FileInfo imageFile, YoloScorer<YoloCocoModel> scorer )
        {
            using var stream = new FileStream(imageFile.FullName, FileMode.Open);

            var image = Image.FromStream(stream);

            List<YoloPrediction> predictions = scorer.Predict(image);

            using var graphics = Graphics.FromImage(image);
            var hasObjects = false;
            foreach (var prediction in predictions) // iterate each prediction to draw results
            {
                hasObjects = true;

                double score = Math.Round(prediction.Score, 2);

                graphics.DrawRectangles(new Pen(prediction.Label.Color, 1),
                    new[] { prediction.Rectangle });

                var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);

                if (y < 1)
                    y += prediction.Rectangle.Height;

                graphics.DrawString($"{prediction.Label.Name} ({score})",
                    new Font("Consolas", 16, GraphicsUnit.Pixel), new SolidBrush(prediction.Label.Color),
                    new PointF(x, y));
            }

            if (hasObjects)
            {
                var outFile = $"{Path.GetFileNameWithoutExtension(imageFile.Name)}_result.jpg";
                Console.WriteLine($"Found objects in {outFile}");
                image.Save($"Assets/{outFile}");
            }
        }
    }
}
