<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hone.aspx.cs" Inherits="WebApplication1.Hone" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gợi ý phim</title>
    <link rel="stylesheet" href="css/style.css" class="css"/>
     <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css"/>
</head>
    <style>
        td{padding: 12px;border: solid 1px #c2c2c2;}
    </style>
<body>
    <div style="background-color:chocolate">
     <nav class="navbar navbar-expand-lg border-bottom box-shadow-nav navbar-fixed-top">
		<%--<a class="navbar-brand" href="#">Demo hệ thống gợi ý lọc cộng tác</a>--%>
          <ul class="navbar-nav mr-auto">
      <li class="nav-item active">
        <a class="nav-link" href="#">Trang chủ<span class="sr-only">(current)</span></a>
      </li>
              <li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          Thể loại
        </a>
        <div class="dropdown-menu" aria-labelledby="navbarDropdown">
          <a class="dropdown-item" href="#">Action</a>
          <a class="dropdown-item" href="#">Adventure</a>
          <a class="dropdown-item" href="#">Children's</a>
          <a class="dropdown-item" href="#">Comedy</a>
          <a class="dropdown-item" href="#">Documentary</a>
        </div>
      </li>
      <li class="nav-item active">
        <a class="nav-link" href="#">Liên hệ<span class="sr-only">(current)</span></a>
      </li>
               
      <li class="nav-item active">
        <a class="nav-link" href="#">Giới thiệu<span class="sr-only">(current)</span></a>
      </li>
              </ul>
		<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarText" aria-controls="navbarText" aria-expanded="false" aria-label="Toggle navigation">
			<i class="fas fa-search"></i>
		</button>

		<div class="collapse navbar-collapse" id="navbarText">
        <form id="form1" runat="server" >
			<div class="navbar-nav mr-auto ml-auto">				
                    <asp:TextBox ID="TextBox1" runat="server" class="form-control nav-textbox-search" placeholder="Nhập ID của USER cần gợi ý" ></asp:TextBox>	
			</div><!-- 
			<ul class="navbar-nav mr-auto ml-auto">
				<li class="nav-item">
					<a class="nav-link" href="#">Home <span class="sr-only">(current)</span></a>
				</li>
				<li class="nav-item">
					<a class="nav-link" href="#">Features</a>
				</li>
				<li class="nav-item">
					<a class="nav-link" href="#">Pricing</a>
				</li>
			</ul> -->
			

		</div>
		<span class="navbar-text">
            <asp:Button ID="submit" class="btn btn-success my-2 my-sm-0" Text="Đăng nhập" runat="server" OnClick="submit_Click" />
				
			</span>
         </form>
	</nav>
        </div>
<br>
	<!-- Begin; List kết quả -->
	<div class="container" style="background-color:bisque">
		<div class="row">
			<div class="col-sd-11 col-md-8 m-auto">

                <%--Danh sách kết quả--%>
                <% WebApplication1.ThuVien tv = new WebApplication1.ThuVien(); %>
                <% if (this.cayco == true && TextBox1.Text.ToString() != "")
                   try{
                       int ID = Int32.Parse(TextBox1.Text.Trim());
                       
                       int[] thoattoan = this.ThuatToan(ID, 23, 101); for (int i = 0; i < 10; i++)
                       {%>
                <div class="card-ketqua w-100">
					<img class="card-img-left" src="hinh.jpg" alt="Card image cap">
					<div class="card-body">
						<h5 class="card-title">
                <% Response.Write(tv.Ham_ExecuteScalar("select TITLE from MOVIES where MOVIEID = "+thoattoan[i].ToString()+"")); %>
                            </h5>

						<a target="_blank" href="<% Response.Write(this.URL_IMDB(thoattoan[i])); %>" class="card-link">imdb.com</a>
						<p class="card-text">
							<% Response.Write("Tag: " +this.Load_The_Loai_Phim(thoattoan[i])); %>
						</p>
						<p class="card-text"><small class="text-muted">Kết quả thứ <% Response.Write((i+1).ToString()); %></small></p>
					</div>
				</div>
				<br><br>
                 <% }
                   } catch(Exception e){}
                    %>
                </div>
		</div>
	</div>
    <footer class="blog-footer text-center border-top m-3" style="padding: 29px;">
      <p>Do PHƯƠNG MAI và NGỌC TUYỀN cùng thực hiện.</p>
      <p>
        <a href="#">Back to top</a>
      </p>
    </footer>
      <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
      <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"></script>
      <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/js/bootstrap.min.js"></script>

</body>
</html>



