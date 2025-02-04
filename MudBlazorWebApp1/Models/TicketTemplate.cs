using System;
using System.Collections.Generic;
namespace MudBlazorWebApp1.Models
{

    public class TicketTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public int AnchoBoleto { get; set; }
        public int AltoBoleto { get; set; }
        public List<TicketElement> Elements { get; set; } = new List<TicketElement>();
    }

    public class TicketElement
    {
        public int Id { get; set; }
        public int TicketTemplateId { get; set; }
        public string ElementType { get; set; }
        public string Content { get; set; }
        public double PosX { get; set; }        
        public double PosY { get; set; }      
        public double Width { get; set; }      
        public double Height { get; set; }     
        public double FontSize { get; set; }   
        public string FontFamily { get; set; }
        public string FontColor { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public double Rotation { get; set; }    
        public int ZIndex { get; set; }
        public bool IsVisible { get; set; }
    }
    public class DatosEvento
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public int CodigoAsiento { get; set; }
        public string Temporada { get; set; }
    }

}




