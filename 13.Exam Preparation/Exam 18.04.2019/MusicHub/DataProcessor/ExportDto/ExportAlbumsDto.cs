using System;
using MusicHub.Data.Models;

namespace MusicHub.DataProcessor.ExportDto
{
    public class ExportAlbumsDto
    {
        public string AlbumName { get; set; }

        public string ReleaseDate { get; set; }

        public string ProducerName { get; set; }

        public SongDto[] Songs { get; set; }

        public decimal AlbumPrice { get; set; }
    }

    public class SongDto
    {
        public string SongName { get; set; }

        public decimal Price { get; set; }

        public string Writer { get; set; }
    }
}
//"AlbumName": "Devil's advocate",
//    "ReleaseDate": "07/21/2018",
//    "ProducerName": "Evgeni Dimitrov",
//    "Songs": [
//      {
//        "SongName": "Numb",
//        "Price": "13.99",
//        "Writer": "Kara-lynn Sharpous"
//      },
//      {
//        "SongName": "Ibuprofen",
//        "Price": "26.50",
//        "Writer": "Stanford Daykin"
//      }
//    ],
//    "AlbumPrice": "40.49"
