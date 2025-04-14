using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using MudBlazorWebApp1.Models;

namespace MudBlazorWebApp1.Services
{
    public class PdfHelper
    {
        public static byte[] GenerateBoletoFromTemplate(TicketTemplate template, DatosEvento datosEvento)
        {
            // Escala: 1 px ≈ 0.75 pt
            float escala = 0.75f;

            using var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);

            // Aplicar la escala al tamaño del boleto
            var pageSize = new PageSize(template.AnchoBoleto * escala, template.AltoBoleto * escala);
            var page = pdf.AddNewPage(pageSize);
            var canvas = new PdfCanvas(page);

            // Cargar fuentes
            var fontRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            foreach (var element in template.Elements)
            {
                // Aplicar la escala a las coordenadas
                float x = (float)(element.PosX * escala);
                float y = (float)((template.AltoBoleto - element.PosY) * escala);

                // Obtener contenido dinámico o estático
                string dynamicValue = element.Content switch
                {
                    "Nombre" => datosEvento.Nombre,
                    "Codigo Asiento" => datosEvento.CodigoAsiento.ToString(),
                    "Temporada" => datosEvento.Temporada,
                    _ => element.Content
                };

                // Texto que muestra el campo + su valor
                string contentToShow = $"{element.Content}: {dynamicValue}";

                // Ajustar tamaño de fuente, aplicar escala si quieres que el font size también lo respete
                float fontSize = (float)(element.FontSize > 0 ? element.FontSize * escala : 10);

                // Colores y formato (puedes extenderlo si ocupas más propiedades)
                var font = element.IsBold ? fontBold : fontRegular;

                // Dibujar el texto
                canvas.BeginText()
                      .SetFontAndSize(font, fontSize)
                      .MoveText(x, y)
                      .ShowText(contentToShow)
                      .EndText();
            }

            pdf.Close();
            return memoryStream.ToArray();
        }
    }
}
