using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;


namespace WebApplication1
{
    public class ThuVien
    {
        string sql_con = "Data Source=NUIVANG\\SQLEXPRESS;Initial Catalog=DA1C;User ID=sa;Password='sa2012'";
        static SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter da;

        
        public void OCConnection()
        {
            try
            {
                conn = new SqlConnection(sql_con);
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                else
                    conn.Open();
            }
            catch (Exception e)
            {
                //Không làm gì
            }
        }

        //Hàm lấy dữ liệu Datatable
        public DataTable Dulieu(string sql)
        {

            try
            {
                OCConnection();


                DataTable tb = new DataTable();
                //cmd = new SqlCommand(sql,conn);
                da = new SqlDataAdapter(sql, conn);
                //SqlDataReader dr = cmd.ExecuteReader();
                //tb.Load(dr);
                da.Fill(tb);
                OCConnection();
                return tb;
            }
            catch (Exception e)
            {
                return new DataTable();
            }
        }
        //Hàm ExecuteNonQuery
        public string Ham_ExecuteScalar(string sql)
        {
            string result = "";
            try
            {
                //Mở kết nối
                OCConnection();

                cmd = new SqlCommand(sql, conn);

                result = cmd.ExecuteScalar().ToString();

                //Đóng kết nối
                OCConnection();

                //Trả về ô đầu tiên

            }
            catch (Exception e) {  }
            return result;
        }
        //Hàm tách table 1 chiều ra mảng 1 chiều
        public int[] Table_To_Array_1(DataTable tb)
        {
            int[] mang = new int[tb.Rows.Count];
            int i = 0;
            foreach (DataRow rating in tb.Rows)
            {
                mang[i] = int.Parse(rating[0].ToString());
                i++;
            }
            return mang;
        }
        //Hàm trả về chuỗi các phần tử trong mảng 1 chiều cách nhau bởi khoản trắng
        public string Xuat_Mang_1_Chieu(int[] mang)
        {
            string result = "";

            foreach (int j in mang)
                result += " " + j.ToString();

            return result;
        }
        //Hàm trả về chuỗi các phần tử trong mảng 1 chiều float cách nhau bởi khoản trắng
        public string Xuat_Mang_1_Chieu_Double(double[] mang)
        {
            string result = "";

            foreach (double j in mang)
                result += " " + j.ToString();

            return result;
        }
        //Hàm xóa các phần tử trùng trong mảng 1 chiều
        public int[] Xoatrung(int[] i_mang)
        {
            List<int> i_list = new List<int>();

            foreach (int j in i_mang)
                i_list.Add(j);

            //Xóa trùng
            i_list = i_list.Distinct().ToList();

            //Thêm list vào mảng
            int[] mang = new int[i_list.Count];
            int i = 0;
            foreach (int j in i_list)
            {
                mang[i] = j;
                i++;
            }

            //Trả về mảng
            return mang;
        }
        //Hàm trả về mảng 2 chiều từ 2 mảng 1 chiều
        public float[,] Return_2Chieu(int[] mang_u, int[] mang_i)
        {
            float[,] mang_ui = new float[mang_u.Length, mang_i.Length];
            string sql_ui;
            int user = 0;
            int item = 0;
            float rating;
            string s;
            DataTable tb;
            foreach (int u in mang_u)
            {
                sql_ui = "select MOVIEID, RATING from RATINGS where USERID = " + u.ToString();
                tb = Dulieu(sql_ui);
                if (tb.Rows.Count == 0)
                    continue;
                item = 0;
                foreach (int i in mang_i)
                {

                    s = (tb.Compute("sum(RATING)", "MOVIEID = '" + i.ToString() + "'")).ToString();
                    if (s != "")
                        rating = float.Parse(s);
                    else
                        rating = 0;
                    mang_ui[user, item] = rating;
                    item++;
                }
                user++;
            }
            return mang_ui;
        }
        //Hàm xuất mảng 2 chiều ra thành bảng
        public string Xuat_Mang_2Chieu(float[,] mang_ui)
        {
            string mang2chieu = "<table>";

            //GetLenght(0) trả về số dòng của mảng 2 chiều = USER
            //GetLenght(1) trả về số cột của mảng 2 chiều == Item

            for (int d = 0; d < mang_ui.GetLength(0); d++)
            {
                mang2chieu += "<tr>";
                for (int c = 0; c < mang_ui.GetLength(1); c++)
                    mang2chieu += "<td>" + mang_ui[d, c].ToString() + "</td>";
                mang2chieu += "</tr>";
            }
            mang2chieu += "</table>";

            return mang2chieu;
        }
        public string Xuat_Mang_2Chieu_Double(double [,] mang_ui)
        {
            string mang2chieu = "<table>";

            //GetLenght(0) trả về số dòng của mảng 2 chiều = USER
            //GetLenght(1) trả về số cột của mảng 2 chiều == Item

            for (int d = 0; d < mang_ui.GetLength(0); d++)
            {
                mang2chieu += "<tr>";
                for (int c = 0; c < mang_ui.GetLength(1); c++)
                    mang2chieu += "<td>" + mang_ui[d, c].ToString() + "</td>";
                mang2chieu += "</tr>";
            }
            mang2chieu += "</table>";

            return mang2chieu;
        }
        //Hàm tính độ tương đồng giữa 2 người dùng i và j bên trong mảng mang_ui
        public double Tinh_Pearsion(int i, int j, float[,] mang_ui)
        {
            if (i == j) return 1;
            double Tu_So = 0;
            double Mau_So_1 = 0;
            double Mau_So_2 = 0;
            double r_AVG_i = 0;
            double r_AVG_j = 0;

            //Tính r_AVG_i & r_AVG_j;
            r_AVG(ref r_AVG_i, ref r_AVG_j, mang_ui, i, j);

            //Tính tử số
            for (int k = 0; k < mang_ui.GetLength(1); k++)
            {
                if (mang_ui[i, k] != 0 && mang_ui[j, k] != 0)
                {
                    Tu_So += (mang_ui[i, k] - r_AVG_i) * (mang_ui[j, k] - r_AVG_j);
                    Mau_So_1 += (mang_ui[i, k] - r_AVG_i) * (mang_ui[i, k] - r_AVG_i);
                    Mau_So_2 += (mang_ui[j, k] - r_AVG_j) * (mang_ui[j, k] - r_AVG_j);
                }
            }

            if (Tu_So == 0)
                return 0;

            
            Mau_So_1 = Math.Sqrt(Mau_So_1);
            Mau_So_2 = Math.Sqrt(Mau_So_2);

            return Tu_So/(Mau_So_1*Mau_So_2);
        }

        //Hàm tính r_AVG
        public void r_AVG(ref double r_AVG_i, ref double r_AVG_j, float[,] mang_ui, int i, int j)
        {
            r_AVG_i = r_AVG_j = 0;
            int dem = 0;
            for (int c = 0; c < mang_ui.GetLength(1); c++)
            {
                if (mang_ui[i, c] != 0 && mang_ui[j, c] != 0)
                {
                    //Cả 2 cùng khác 0 thì tính
                    r_AVG_i += mang_ui[i, c];
                    r_AVG_j += mang_ui[j, c];

                    dem++;
                }
            }

            //Ra khỏi vòng lặp thì chia cho trung bình;
            r_AVG_i /= dem;
            r_AVG_j /= dem;

            //Làm tròn số
            //r_AVG_i = Math.Round(r_AVG_i, 3);
            //r_AVG_j = Math.Round(r_AVG_j, 3);
        }
        //Hàm trả về 2 người dùng gần với người dùng id_user nhất
        public void Return_2_Nguoi_Dung_Gan_Nhat(int id_user, ref int max1, ref int max2, double[,] Array_Pearsion)
        {
            max1 = 0;
            for(int i = 0; i < Array_Pearsion.GetLength(0); i++)
            {
                
                    if (Array_Pearsion[id_user, i] > Array_Pearsion[id_user, max1])
                        if (id_user != i)
                            max1 = i;
                
            }
            max2 = 0;
            for(int i = 0; i< Array_Pearsion.GetLength(0); i++)
            {
               
                if (Array_Pearsion[id_user, i] > Array_Pearsion[id_user, max2])
                    if (id_user != i && i != max1)
                        max2 = i;
                
            }   
        }
        //Hàm trả về mảng 2 chiều chứa các phần tử giá trị trung bình xếp hạng cho từng sản phẩm
        public float[,] Tinh_AVG_Ratings_Item(float[,] mang_ui)
        {
            float[,] Array_AVG_Ratings_Item = mang_ui;
            int d = Array_AVG_Ratings_Item.GetLength(0);
            int c = Array_AVG_Ratings_Item.GetLength(1);

            for (int i = 0; i < d; i++)
            {
                float avg = this.Tinh_AVG_Of_Item(Array_AVG_Ratings_Item, i);
                if(avg != 0)
                for (int j = 0; j < c; j++)
                {
                    if (Array_AVG_Ratings_Item[i, j] != 0)
                    Array_AVG_Ratings_Item[i,j] = (avg - Array_AVG_Ratings_Item[i,j]);
                }
            }
            return Array_AVG_Ratings_Item;
        }
        //Viết hàm tính giá trị Ratings trung bình chỉ dựa vào dòng của User chứ ko phụ thuộc vào dòng User khác
        public float Tinh_AVG_Of_Item(float[,] mang, int d)
        {
            float tong = 0;
            int dem = 0;
            int soluong = 0;
            int i = mang.GetLength(1);
            for (; dem < i; dem++)
            {
                if (mang[d, dem] != 0)
                {
                    soluong++;
                    tong += mang[d, dem];
                }
            }
           

            if (soluong == 0)
                return 0;
            return tong / (float)soluong;
        }
        //Viết hàm tính list AVG Ratings cho người dùng inpput
        public double[] Goi_Y_Gia_Tri_Trung_Binh_Cho_User_Input(float[,] Array_AVG_Ratings_Item, double[,] Array_Pearsion, int max1, int max2, int index_user_id, double avg_ofuser)
        {
            int c = Array_AVG_Ratings_Item.GetLength(1);

            double avg_u = avg_ofuser;
            double[] result = new double[c];
            for (int i = 0; i < c; i++)
            {
                if (Array_AVG_Ratings_Item[index_user_id, i] == 0)
                {

                    double _in = (double)avg_u + (((double)Array_AVG_Ratings_Item[max1, i] * Array_Pearsion[max1, index_user_id] + (double)Array_AVG_Ratings_Item[max2, i] * Array_Pearsion[max2, index_user_id]) / (Array_Pearsion[max1, index_user_id] + Array_Pearsion[max2, index_user_id]));
                    result[i] = _in;
                }
                else
                    result[i] = (double)Array_AVG_Ratings_Item[index_user_id, i];
            }
           
            return result;
        }
        //Sắp xếp mảng double giảm dần
        public double[] SapXepMang(ref int[] mangi, double[] mang)
        {
            int n = mang.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (mang[j] > mang[i])
                    {
                        //cach trao doi gia tri
                        double tmp = mang[i];
                        int tmpi = mangi[i];

                        mang[i] = mang[j];
                        mang[j] = tmp;

                        mangi[i] = mangi[j];
                        mangi[j] = tmpi;
                    }
                }
            }
            return mang;
        }
    }
}