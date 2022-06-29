using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IRecognitionService
    {
        Task<bool> BarcodeInfo(string barcode);
        Task<bool> Get100gInfo(string name);
        Task<bool> NaturalInfo(string query);
        void Upload(HttpRequest request);
    }
}
