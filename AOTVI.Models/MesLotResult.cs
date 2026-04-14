using System.Collections.Generic;
namespace AOTVI.Models
{
    public class MesLotResult
    {
        public string LotNumber { get; set; }
        public string Result { get; set; }
        public string User { get; set; }
        public List<Defect> Defects { get; set; }
    }
}