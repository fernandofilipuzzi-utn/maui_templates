using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonMaui;

public interface IImageDevice
{
    public int MaxWidthHeight { get; set; }

    public int CompressionQuality { get; set; }

    public double CustomPhotoSize { get; set; }

}