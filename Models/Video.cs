using System.ComponentModel.DataAnnotations;

namespace T2_Videoteca_RevillaFernandezHectorRolando.Models
{
    public class Video
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? tipo { get; set; }
        public int veces_vista { get; set; }
        public decimal rating { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fecha { get; set; }
    }
}
