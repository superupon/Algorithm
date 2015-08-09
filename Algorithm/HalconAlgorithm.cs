using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Drawing;
using System.Drawing.Imaging;
namespace Algorithm
{

    class GenerateFuctions
    {
        public static void Normal_Train_Init(HObject ho_Image, out HObject ho_Circle, HTuple hv_Row1,
            HTuple hv_Column1, HTuple hv_Row2, HTuple hv_Column2, HTuple hv_Row, HTuple hv_Column,
            HTuple hv_Radius, out HTuple hv_ModelID, out HTuple hv_rstd, out HTuple hv_cstd,
            out HTuple hv_VmodleR, out HTuple hv_VmodleG, out HTuple hv_VmodleB)
        {



            // Local iconic variables 

            HObject ho_Rectangle, ho_ImageReduced;


            // Local control variables 

            HTuple hv_Area = null, hv_Width = null, hv_Height = null;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);

            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle1(out ho_Rectangle, hv_Row1, hv_Column1, hv_Row2, hv_Column2);
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Rectangle, out ho_ImageReduced);
            HOperatorSet.CreateShapeModel(ho_ImageReduced, "auto", -((new HTuple(-180)).TupleRad()
                ), (new HTuple(360)).TupleRad(), "auto", "auto", "use_polarity", "auto",
                "auto", out hv_ModelID);
            HOperatorSet.AreaCenter(ho_Rectangle, out hv_Area, out hv_rstd, out hv_cstd);
            ho_Circle.Dispose();
            HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, hv_Radius);

            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.CreateVariationModel(hv_Width, hv_Height, "byte", "standard", out hv_VmodleR);
            HOperatorSet.CreateVariationModel(hv_Width, hv_Height, "byte", "standard", out hv_VmodleG);
            HOperatorSet.CreateVariationModel(hv_Width, hv_Height, "byte", "standard", out hv_VmodleB);
            ho_Rectangle.Dispose();
            ho_ImageReduced.Dispose();

            return;
        }

        public static void Normal_Train(HObject ho_Image, HObject ho_Circle, HTuple hv_ModelID,
            HTuple hv_rstd, HTuple hv_cstd, HTuple hv_VmodleR, HTuple hv_VmodleG, HTuple hv_VmodleB)
        {



            // Local iconic variables 

            HObject ho_ImageReduced1, ho_ImageAffinTrans = null;
            HObject ho_Image1 = null, ho_Image2 = null, ho_Image3 = null;


            // Local control variables 

            HTuple hv_Row3 = null, hv_Column3 = null, hv_Angle = null;
            HTuple hv_Score = null, hv_HomMat2D = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Image2);
            HOperatorSet.GenEmptyObj(out ho_Image3);

            ho_ImageReduced1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ImageReduced1);
            HOperatorSet.FindShapeModel(ho_ImageReduced1, hv_ModelID, -((new HTuple(-180)).TupleRad()
                ), (new HTuple(360)).TupleRad(), 0.5, 1, 0.5, "least_squares", 0, 0.9, out hv_Row3,
                out hv_Column3, out hv_Angle, out hv_Score);
            if ((int)(new HTuple(((hv_Score.TupleSelect(0))).TupleGreater(0.5))) != 0)
            {
                HOperatorSet.VectorAngleToRigid(hv_Row3, hv_Column3, hv_Angle, hv_rstd, hv_cstd,
                    0, out hv_HomMat2D);
                ho_ImageAffinTrans.Dispose();
                HOperatorSet.AffineTransImage(ho_Image, out ho_ImageAffinTrans, hv_HomMat2D,
                    "bilinear", "false");

                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                HOperatorSet.Decompose3(ho_ImageAffinTrans, out ho_Image1, out ho_Image2, out ho_Image3
                    );

                HOperatorSet.TrainVariationModel(ho_Image1, hv_VmodleR);
                HOperatorSet.TrainVariationModel(ho_Image2, hv_VmodleG);
                HOperatorSet.TrainVariationModel(ho_Image3, hv_VmodleB);
            }
            else
            {






            }

            ho_ImageReduced1.Dispose();
            ho_ImageAffinTrans.Dispose();
            ho_Image1.Dispose();
            ho_Image2.Dispose();
            ho_Image3.Dispose();

            return;
        }

        public static void Normal_Finish_Init(out HObject ho_minim, out HObject ho_maxim, HTuple hv_VmodleR,
            HTuple hv_VmodleG, HTuple hv_VmodleB)
        {


            // Local iconic variables 

            HObject ho_MinR, ho_MaxR, ho_MinG, ho_MaxG;
            HObject ho_MinB, ho_MaxB;

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_minim);
            HOperatorSet.GenEmptyObj(out ho_maxim);
            HOperatorSet.GenEmptyObj(out ho_MinR);
            HOperatorSet.GenEmptyObj(out ho_MaxR);
            HOperatorSet.GenEmptyObj(out ho_MinG);
            HOperatorSet.GenEmptyObj(out ho_MaxG);
            HOperatorSet.GenEmptyObj(out ho_MinB);
            HOperatorSet.GenEmptyObj(out ho_MaxB);

            HOperatorSet.PrepareVariationModel(hv_VmodleR, 16, 4);
            ho_MinR.Dispose();
            ho_MaxR.Dispose();
            HOperatorSet.GetThreshImagesVariationModel(out ho_MinR, out ho_MaxR, hv_VmodleR);


            HOperatorSet.PrepareVariationModel(hv_VmodleG, 16, 4);
            ho_MinG.Dispose();
            ho_MaxG.Dispose();
            HOperatorSet.GetThreshImagesVariationModel(out ho_MinG, out ho_MaxG, hv_VmodleG);

            HOperatorSet.PrepareVariationModel(hv_VmodleB, 16, 4);
            ho_MinB.Dispose();
            ho_MaxB.Dispose();
            HOperatorSet.GetThreshImagesVariationModel(out ho_MinB, out ho_MaxB, hv_VmodleB);


            ho_minim.Dispose();
            HOperatorSet.Compose3(ho_MinR, ho_MinG, ho_MinB, out ho_minim);

            ho_maxim.Dispose();
            HOperatorSet.Compose3(ho_MaxR, ho_MaxG, ho_MaxB, out ho_maxim);

            ho_MinR.Dispose();
            ho_MaxR.Dispose();
            ho_MinG.Dispose();
            ho_MaxG.Dispose();
            ho_MinB.Dispose();
            ho_MaxB.Dispose();

            return;
        }

        public static void Normal_Inspect(HObject ho_Image, HObject ho_Circle, out HObject ho_ImageAffinTrans,
            out HObject ho_SelectedRegions, HTuple hv_ModelID, HTuple hv_rstd, HTuple hv_cstd,
            HTuple hv_VmodleR, HTuple hv_VmodleG, HTuple hv_VmodleB, HTuple hv_minarea)
        {



            // Local iconic variables 

            HObject ho_ImageReduced1, ho_ImageReduced2 = null;
            HObject ho_Image1 = null, ho_Image2 = null, ho_Image3 = null;
            HObject ho_RegionDiff1 = null, ho_RegionDiff2 = null, ho_RegionDiff3 = null;
            HObject ho_RegionUnion = null, ho_RegionUnion1 = null, ho_ConnectedRegions = null;


            // Local control variables 

            HTuple hv_Row3 = null, hv_Column3 = null, hv_Angle = null;
            HTuple hv_Score = null, hv_HomMat2D = new HTuple();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced2);
            HOperatorSet.GenEmptyObj(out ho_Image1);
            HOperatorSet.GenEmptyObj(out ho_Image2);
            HOperatorSet.GenEmptyObj(out ho_Image3);
            HOperatorSet.GenEmptyObj(out ho_RegionDiff1);
            HOperatorSet.GenEmptyObj(out ho_RegionDiff2);
            HOperatorSet.GenEmptyObj(out ho_RegionDiff3);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion1);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);

            ho_ImageReduced1.Dispose();
            HOperatorSet.ReduceDomain(ho_Image, ho_Circle, out ho_ImageReduced1);
            HOperatorSet.FindShapeModel(ho_ImageReduced1, hv_ModelID, -((new HTuple(-180)).TupleRad()
                ), (new HTuple(360)).TupleRad(), 0.5, 1, 0.5, "least_squares", 0, 0.9, out hv_Row3,
                out hv_Column3, out hv_Angle, out hv_Score);
            if ((int)(new HTuple(((hv_Score.TupleSelect(0))).TupleGreater(0.5))) != 0)
            {
                HOperatorSet.VectorAngleToRigid(hv_Row3, hv_Column3, hv_Angle, hv_rstd, hv_cstd,
                    0, out hv_HomMat2D);
                ho_ImageAffinTrans.Dispose();
                HOperatorSet.AffineTransImage(ho_Image, out ho_ImageAffinTrans, hv_HomMat2D,
                    "bilinear", "false");
                ho_ImageReduced2.Dispose();
                HOperatorSet.ReduceDomain(ho_ImageAffinTrans, ho_Circle, out ho_ImageReduced2
                    );
                ho_Image1.Dispose();
                ho_Image2.Dispose();
                ho_Image3.Dispose();
                HOperatorSet.Decompose3(ho_ImageReduced2, out ho_Image1, out ho_Image2, out ho_Image3
                    );
                ho_RegionDiff1.Dispose();
                HOperatorSet.CompareVariationModel(ho_Image1, out ho_RegionDiff1, hv_VmodleR);
                ho_RegionDiff2.Dispose();
                HOperatorSet.CompareVariationModel(ho_Image2, out ho_RegionDiff2, hv_VmodleG);
                ho_RegionDiff3.Dispose();
                HOperatorSet.CompareVariationModel(ho_Image3, out ho_RegionDiff3, hv_VmodleB);

                ho_RegionUnion.Dispose();
                HOperatorSet.Union2(ho_RegionDiff1, ho_RegionDiff2, out ho_RegionUnion);
                ho_RegionUnion1.Dispose();
                HOperatorSet.Union2(ho_RegionUnion, ho_RegionDiff3, out ho_RegionUnion1);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionUnion1, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", hv_minarea, 9999999);

            }
            else
            {
                //*定位失败


            }
            ho_ImageReduced1.Dispose();
            ho_ImageReduced2.Dispose();
            ho_Image1.Dispose();
            ho_Image2.Dispose();
            ho_Image3.Dispose();
            ho_RegionDiff1.Dispose();
            ho_RegionDiff2.Dispose();
            ho_RegionDiff3.Dispose();
            ho_RegionUnion.Dispose();
            ho_RegionUnion1.Dispose();
            ho_ConnectedRegions.Dispose();

            return;
        }
    }
    public class HalconAlgorithm
    {
        // Procedures 
        // Local procedures 
        public static void Halcon_Train(string bitmap_train_init, double row1, double column1, 
            double row2, double column2, double row, double column, double radius, string train_bitmaps, int times,
            out byte[] minimum, out byte[] maximum)
        {
            HTuple ModelID = null;
            HTuple rstd = null, cstd = null;
            HTuple VmodleR = null,VmodleG = null,VmodleB = null;
            
            
            //GenerateFuctions.Normal_Train_Init();
            Bitmap bitmap = new Bitmap(bitmap_train_init);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            IntPtr pBitmap = bmData.Scan0;
            //pPixels = pBitmap; 

            HImage hImage = new HImage();
            hImage.GenImage1("byte", bitmap.Width, bitmap.Height, pBitmap);


            bitmap.UnlockBits(bmData);
            bitmap.Dispose();
            HObject circle = null;
            HOperatorSet.GenEmptyObj(out circle);

            GenerateFuctions.Normal_Train_Init(hImage, out circle, row1, column1, row2, column2, row, column, radius,
                out ModelID, out  rstd, out cstd, out VmodleR, out VmodleG, out VmodleB);
        }

        public static void Halcon_Inspect(string image, double row, double column, double radius, out byte[] imageAffinTrans,
            out byte[] selectedRegions, int ModelID, double rstd, double cstd,
            double VmodleR, double VmodleG, double VmodleB, double minarea)
        {
            
            
        }
        public static void Main()
        {
            Bitmap bitmap = new Bitmap(@"C:\Users\Jacob\Downloads\deco\deco\MANUAL_20100514143231_0_0000_R.bmp");
            Rectangle rect = new Rectangle(0,0, bitmap.Width, bitmap.Height);
            BitmapData bmData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed); 
            IntPtr pBitmap = bmData.Scan0; 
            //pPixels = pBitmap; 

            HImage hImage = new HImage();
            hImage.GenImage1("byte", bitmap.Width, bitmap.Height, pBitmap); 

            
            bitmap.UnlockBits(bmData); 

            HObject circle = null;
            HOperatorSet.GenEmptyObj(out circle);
            HTuple ModelID;
            HTuple rstd;
            HTuple cstd;
            HTuple VmodleR,VmodleG,VmodleB;
            
            GenerateFuctions.Normal_Train_Init(hImage,out circle, 27.6, 48.7, 239.1, 338.3, 139, 186.6, 85.1, 
                out ModelID,out  rstd, out cstd, out VmodleR, out VmodleG, out VmodleB);
            

        }
    }
}
