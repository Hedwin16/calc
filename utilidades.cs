using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using Suite.ServicioSuite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TfhkaNet.IF;
using TfhkaNet.IF.VE;

namespace Suite
{
    public class utilidades
    {
        public static void BarraProgreso(XtraForm form, bool accion, string tarea)
        {
            if (accion == true)
            {
                //Open Wait Form
                SplashScreenManager.ShowForm(form, typeof(xfrm_VentanaEspera), false, false, false);
                SplashScreenManager.Default.SetWaitFormDescription(tarea);
            }
            else
            {
                //Close Wait Form
                SplashScreenManager.CloseForm(false);
            }
        }

        public static DialogResult MensajeError(string Mensaje1, string Mensaje2, string Mensaje3, string Titulo, string TipoMensaje)
        {
            try
            {
                string MensajeFinal;

                MensajeFinal = Mensaje1;

                if (Mensaje2 != null)
                {
                    MensajeFinal = MensajeFinal + Environment.NewLine + Environment.NewLine + "Detalle:" + Environment.NewLine + Environment.NewLine + Mensaje2;
                }

                if (Mensaje3 != null)
                {
                    MensajeFinal = MensajeFinal + Environment.NewLine + Environment.NewLine + "Detalle:" + Environment.NewLine + Environment.NewLine + Mensaje3;
                }

                DialogResult mensaje;

                switch (TipoMensaje)
                {
                    case "Pregunta":

                        mensaje = MessageBox.Show(MensajeFinal, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        return mensaje;

                    case "Información":

                        mensaje = MessageBox.Show(MensajeFinal, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return mensaje;

                    case "Error":

                        mensaje = MessageBox.Show(MensajeFinal, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return mensaje;

                    case "Advertencia":

                        mensaje = MessageBox.Show(MensajeFinal, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        return mensaje;

                    default:
                        break;
                }

                return DialogResult.Abort;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error. No se puede mostrar los detalles de la excepción." + Environment.NewLine + Environment.NewLine + ex.Message, "Suite - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return DialogResult.Abort;
            }
        }

        public static string querySucursal(string codigoEmpresa, string codigoSucursal, string descripcion)
        {
            string q = @"ALTER DATABASE [suiteMasterDB] SET COMPATIBILITY_LEVEL = 120
                        
                        IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
                        begin
                        EXEC [suiteMasterDB].[dbo].[sp_fulltext_database] @action = 'enable'
                        end
                        
                        ALTER DATABASE [suiteMasterDB] SET ANSI_NULL_DEFAULT OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET ANSI_NULLS OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET ANSI_PADDING OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET ANSI_WARNINGS OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET ARITHABORT OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET AUTO_CLOSE OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET AUTO_SHRINK OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET AUTO_UPDATE_STATISTICS ON 
                        
                        ALTER DATABASE [suiteMasterDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET CURSOR_DEFAULT  GLOBAL 
                        
                        ALTER DATABASE [suiteMasterDB] SET CONCAT_NULL_YIELDS_NULL OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET NUMERIC_ROUNDABORT OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET QUOTED_IDENTIFIER OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET RECURSIVE_TRIGGERS OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET  ENABLE_BROKER 
                        
                        ALTER DATABASE [suiteMasterDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET TRUSTWORTHY OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET PARAMETERIZATION SIMPLE 
                        
                        ALTER DATABASE [suiteMasterDB] SET READ_COMMITTED_SNAPSHOT ON 
                        
                        ALTER DATABASE [suiteMasterDB] SET HONOR_BROKER_PRIORITY OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET RECOVERY FULL 
                        
                        ALTER DATABASE [suiteMasterDB] SET  MULTI_USER 
                        
                        ALTER DATABASE [suiteMasterDB] SET PAGE_VERIFY CHECKSUM  
                        
                        ALTER DATABASE [suiteMasterDB] SET DB_CHAINING OFF 
                        
                        ALTER DATABASE [suiteMasterDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
                        
                        ALTER DATABASE [suiteMasterDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
                        
                        ALTER DATABASE [suiteMasterDB] SET DELAYED_DURABILITY = DISABLED 
                        
                        EXEC sys.sp_db_vardecimal_storage_format N'suiteMasterDB', N'ON'
                        
                        USE [suiteMasterDB]
                        SET ANSI_NULLS ON
                        
                        SET QUOTED_IDENTIFIER ON
                        
                        SET ANSI_PADDING ON
                        
                        CREATE TABLE [dbo].[__MigrationHistory](
	                        [MigrationId] [nvarchar](150) NOT NULL,
	                        [ContextKey] [nvarchar](300) NOT NULL,
	                        [Model] [varbinary](max) NOT NULL,
	                        [ProductVersion] [nvarchar](32) NOT NULL,
                         CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
                        (
	                        [MigrationId] ASC,
	                        [ContextKey] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                        
                        SET ANSI_PADDING OFF

                        SET ANSI_NULLS ON
                        
                        SET QUOTED_IDENTIFIER ON
                        
                        SET ANSI_PADDING ON
                        
                        CREATE TABLE [dbo].[empresas](
	                        [codigo] [varchar](20) NOT NULL,
	                        [descripcion] [varchar](50) NULL,
	                        [activo] [bit] NOT NULL,
                         CONSTRAINT [PK_dbo.empresas] PRIMARY KEY CLUSTERED 
                        (
	                        [codigo] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]

                        
                        SET ANSI_PADDING OFF
                        SET ANSI_NULLS ON
                        
                        SET QUOTED_IDENTIFIER ON
                        
                        SET ANSI_PADDING ON
                        
                        CREATE TABLE [dbo].[sucursales](
	                        [codigo] [varchar](20) NOT NULL,
	                        [empresa] [varchar](20) NOT NULL,
	                        [descripcion] [varchar](50) NULL,
	                        [activo] [bit] NOT NULL,
                         CONSTRAINT [PK_dbo.sucursales] PRIMARY KEY CLUSTERED 
                        (
	                        [codigo] ASC,
	                        [empresa] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]

                        
                        SET ANSI_PADDING OFF
                        SET ANSI_NULLS ON
                        
                        SET QUOTED_IDENTIFIER ON
                        
                        SET ANSI_PADDING ON
                        
                        CREATE TABLE [dbo].[usuarios](
	                        [codigo] [varchar](20) NOT NULL,
	                        [descripcion] [varchar](50) NOT NULL,
	                        [password] [varchar](8000) NOT NULL,
	                        [acceso] [varchar](8000) NULL,
	                        [activo] [bit] NOT NULL,
                         CONSTRAINT [PK_dbo.usuarios] PRIMARY KEY CLUSTERED 
                        (
	                        [codigo] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]

                        
                        SET ANSI_PADDING OFF
                        
                        INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202003170000168_AutomaticMigration', N'Suite.EntidadesMaster.Migrations.Configuration', 0x1F8B0800000000000400ED5ADB6EDC36107D2FD07F10F45838ABB58300A9B19BC059DB85D1D80E2C3BE89BC19566D74429522529671745BFAC0FFDA4FE4247374AA2A4BDD94EE2A4C84B56E41C0E87E7CC8823FFFBF73FA3B78B8839F72015157CECEE0F86AE033C1021E5F3B19BE8D98BD7EEDB373FFE303A09A385F3B19CF7329D87965C8DDD3BADE343CF53C11D44440D221A48A1C44C0F0211792414DEC170F8B3B7BFEF0142B888E538A3AB846B1A41F6037F4E040F20D60961E72204A68AE738E267A8CE058940C52480B1EB2754C3E004ED4312823A274A83749D2346093AE3039BB90EE15C68A2D1D5C31B05BE9682CFFD181F1076BD8C01E7CD0853506CE1B09ABEE96E8607E96EBCCAB0840A12A545B425E0FECB223C9E6DBE53905D133E0C601A28BD4C779D0571EC42144B50C475ECB50E274CA6F37A423C280CF79CCEE13D430EE450FA6FCF99244C2712C61C122D09DB733E245346835F61792D7E073EE60963755FD15B1C6B3CC0471FA48841EAE515CC8A1DA4EC9C0BD7F19AB69E6D6C4C2DBB62935A22C75DE79C2CDE039FEBBBB17B80A43EA50B08CB07054B6E38455BB4D132C19F17E837993230E3B518FA5A48F8053848A221FC4034860649702138B41CB6DCC3500692C64176ECBD3EBEDACCC7D54B9140D37B138977423020BC636316CA05B9A7F36C9F169E9F04895484B9CE15B06C82BAA371AEC6812A066F4F4AE69D4A115D89741D7BECF69AC83968F44CF44CF0452203CBB59157B17C25F795F1735BF297965F2BFB3BA71AA97F634A31FB7A74FFBE59851AED3D54A0A5FE7A055A2A782781262A21928AEDF559187EADF27CF6927B54EA6F29BB9828F549C870C5C2F8DFA758990401A85587B2E9BA8F2FF63E491D2925029A9DB755F48CBC9BBE9CF0D059A7F57CFBF55C81514001D11825834E8CDD9F5A7B5C816B8A7C856B527A1376DFB51577C98F818106E728C8DFAF274405A8F4B6E2D081E613142948E0E92D002F1C0A655FB96E2B9AF280C684AD71DEB2DBAE547B66157BE41862E0213AB9E64C3659BEA7FCA7EB9B65AC98AD0BD1C8ABF1AB9DC9D146A30548433D4CC6790A4E8760A13B323BDED08AE4AE0AB9D8444AA17DD0CD52A65CA75280CDA216179B10E5FB2A748154145F837293D79C2E0C53C72C885AF42A9C76FDAD4DEBABD1F6916E2063E37F33002D766CA0DC1A54751CB6009B9BEDC85D862ED57DDFCB2FFC6563C0EBE90C8CCE491C6332AE750A8A278E9FB709262FFCED2FCF518EE105AAE30E6DBC352B61392573B0467169F4F4944AA58F8926539266EE4918B5A67588A3876FE58216FFDBE756B2B03448FF5F327EC595BE239B1408A7B8C128CD46D9AB43EDD47B2DB39E0D6144F6BCDF4C044B22BE3A3FF6A3345E43EA508D81CDF1CACA5B872A9FB551469E1596565E6D1D41AB26358F74A303AF89F5E1476EB2DBF667DE6FFAB4876EB27A1DA6B7B8FD4F9E06794C917A3875CAA2B63D737A2D9F57B6A82E2275B0EAE936D4C92F164DEAE4CF9E25015B25DD9E625637A5DD2AE1A3A29CAEFF02D0AAAFF914D7C1F0DCD330ADADFE12191B0DD20903FF0F366114F75B4D38279CCE40E9FCAA8F57EBFD03EB0BC2D7D3CDF7940AD9662DFD2FD6AEB82732B823B2D5AF78787FAD0BF9550379FB0BF694EA1D2FD7FD0DE567D1C87DA463B2DAAF06B575B33BC32BE662ECFE99D91D3A67BFDD9A6F4897126578E80C9DBFBE3FD27456C3EF45AE3B75FCBA605F0F87DB03371B7A9BC07E4EA63CA077677F12F95C3DB576AF60C776E14ECDB9FEFBEB9336E39E49F7ADDD8758DDCF321EF7F7D5F2372D94FE34A57B4E70E86B00D9F8151956F4DCBA5650BDDDAAD61AA6F1D6DF92EB5A2131ADBC2FD0B0B3E2DEE8AF6CD29D6B18F6762F1FA51DD77E4747D6D6FE960725A3E8BC8248FFB28743D0E0AB9973C667A2D48DE55139C54AB4E7A04988643E929ACE30EBE230E673957D95F948589275C7A6109EF1CB44C789C62D433465CB7A3052F9AD5A3FEB39367D1E5DC6D927D3C7D802BA89976A0D97FC5D425968FC3EED28133D10A9AE8BAF78E959EAF46BDE7C6990DA9FF3FA808AF09974748DAC6308A62EB94FEE6117DF6E14BC87390996E555AB1F64FD4134C33E3AA6642E49A40A8CCA1E7F2287C368F1E63F8325406ED2260000, N'6.4.0')
                        INSERT [dbo].[__MigrationHistory] ([MigrationId], [ContextKey], [Model], [ProductVersion]) VALUES (N'202003170040489_asdssss', N'Suite.EntidadesMaster.Migrations.Configuration', 0x1F8B0800000000000400ED5A5F6FDB36107F1FB0EF20E871482D2745812EB05BA44E32046B92224A8ABD15B474768851A44652998D619F6C0FFB48FB0A3BFDA3244AB2E5264D9375C84B24F27E3C1E7F77C73BF99FBFFE9EBC5D45CCB903A9A8E053777F34761DE08108295F4EDD442F5EBC76DFBEF9FEBBC94918AD9C8FE5BC97E93C94E46AEADE6A1D1F7A9E0A6E21226A14D1400A25167A1488C823A1F00EC6E31FBDFD7D0F10C2452CC7995C255CD308B2077C9C091E40AC13C2CE45084C15EF71C4CF509D0B12818A490053D74FA886D109CA872404754E9406E93A478C1254C607B6701DC2B9D044A3AA87370A7C2D055FFA31BE20EC7A1D03CE5B10A6A0D8C261357DE86EC607E96EBC4AB0840A12A545B423E0FECBC23C9E2DFE5946768DF9D080A9A1F43ADD7566C4A90B512C4111D7B1D73A9C3199CEEB31F1A810DC733A87F70C399043E9DF9E334B984E244C39245A12B6E77C48E68C063FC3FA5AFC0A7CCA13C6EABAA2B638D67881AF3E481183D4EB2B58143B48D9B914AEE335653D5BD8885A72C526B5448EBBCE3959BD07BED4B753F700497D4A5710962F0A96DC708AB228A365828F17A837993330E3351BFA5A48F8093848A221FC40349A0649702138B414B6D443530692C64176ECBD3ABE1AA6E3E6A548A0E99DB1C43B211810DEB1310BE582DCD165B64F0BCF4F82442AC25CE70A583641DDD238F7C692379FAA49A752445782556434639F7C91C80057BE163D13AE895C826EAA36F12A966FE4BE322AEC4AFE52F2A9B2BF73AA71F5FF98A7987D3DB87ECFDA4307BB41A21222A9D8DD0B0AC1A7EA04CF9ED80F4AB01DC91D13A57E1732DCB030FEFB25562641006AD3A10C5DF7F15CEA482911D0ECBC9B31A9CA724D5D4E78E86C4B79F9F66B29138D80FE4363F418D4016FE7AEED1997FC181868708E82FCB639232A408F6C7B062A3058A332C7D634AA326753A51F5A2BA19382049EDEB5F15AAFD0ED29D76D8FA63CA031615B8C62C9ED96103DB38A3D720C31F01095DC628121CBF724D9747DB38C7516DB4C34F16AFC6A477294D12801D2DC6A3018E721381D8295EE88EC580715C15D15EE62D32185F64117A027F9B694EB541E60B3D3DB0C515A13BA402A426D41B9C9734E1786C9631644CD7A154EFB1A5A9BD67755B58F74801B1BFDEB366C716380F7D580EA96B41DBBB9D98ED865E85255D55E5E5697E5B7D7537F4FCE491C6330AED5E3C51BC7CF8BF1D90B7FF71235CA31BC407554AA465BB312A653B2046B1497464D4FA955FA9868322769E49E85516B5A8773F4F0AD5CD0E27FFBE44A169602E9FF25E33714CE1DD1A44038C50D466934CAAE0EB573EF95CC3A238411D973BF990996447C737CEC47695C43EA508D81E17865E6AD4395EFDA2813CF324B2BAEB68EA095EB9A473AE8C06B3E76FF23573D0164C099F78B7ED9433751BD0ED39BDCFE274F833C2649DD9F3A6552DB9D39BD92CF2B5A5485481DAC7ABB0B75F2C2A2499DFCDDB324602BA5DB53CCEA26B55B297C52A4D3ED7DF6567ECDA7B80E9AE78E86696EF5D7C8D868944E18F9BFB119A3B8DF6AC239E174014AE7A53E96D6FB07569FFEE9F4CC3DA54236AC71FED5DA15774406B744B6FA15F7EF627521BF6A20EF5E60CFA97EA87E55774A7CAAEDD2073A26ABC969505B95DD199698ABA9FB472677E89CFDF2C97CA9B994E88687CED8F9F3DB234D6736FC56DCF5B33A7E5DB0AFC7E3DD819B0DBD21B08FC994FBF7EEBE62AFAE6A9B3C7A73EEB19A711B6AA7A7D87D6BF72136F7B38CC6FD7DB5FCA685AE3F4FE99E131C7A7A492DFC8A0C1B7A6E5D2BA8FE2693BD8669BCF5B7E4BA56484C2BEFEB35EC1A7DB57EF674795C5791FE45DA71ED3B3AB2B6F68B19741945971544FAFB190E4183AF66CE195F88D26F2C8DCA2956A03D074D4224F391D47481511787319EABECABCC47C292AC3B3687F08C5F263A4E346E19A2395BD78D91BADFA6F5B39E6353E7C9659CFD72E021B6806A6251ADE192BF4B280B8DDEA71D69A20722F5EBE22B5E7A963AFD9AB75C1BA4F6E7BC3EA0C27C261C5D23EB1882A94BEE933BF81CDD6E14BC872509D665A9D50FB2FD209A669F1C53B29424520546258F8FC8E1305ABDF9178B64DCFB38260000, N'6.4.0')
                        INSERT [dbo].[empresas] ([codigo], [descripcion], [activo]) VALUES (N'01', N'EMPRESA DE PRUEBA', 1)
                        INSERT [dbo].[sucursales] ([codigo], [empresa], [descripcion], [activo]) VALUES (N'01', N'01', N'SEDE PRINCIPAL', 1)
                        SET ANSI_PADDING ON
                        CREATE NONCLUSTERED INDEX [IX_empresa] ON [dbo].[sucursales]
                        (
	                        [empresa] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        
                        ALTER TABLE [dbo].[sucursales]  WITH CHECK ADD  CONSTRAINT [FK_dbo.sucursales_dbo.empresas_empresa] FOREIGN KEY([empresa])
                        REFERENCES [dbo].[empresas] ([codigo])
                        ON DELETE CASCADE
                        
                        ALTER TABLE [dbo].[sucursales] CHECK CONSTRAINT [FK_dbo.sucursales_dbo.empresas_empresa]
                        
                        USE [master]
                        
                        ALTER DATABASE [suiteMasterDB] SET  READ_WRITE 
                        ";
            return q;
        }

        public static string Estacion_GetName()
        {
            string netBiosName = System.Environment.MachineName;

            //return netBiosName;
            // Following method is deprecated
            // string dnsName =
            //     System.Net.Dns.GetHostByName("LocalHost").HostName;

            string dnsName = System.Net.Dns.GetHostName();
            return dnsName;
        }

        public static void Formulario_Calendario()
        {
            XtraForm xf = new XtraForm();

            CalendarControl cc = new CalendarControl();           
            cc.Dock = DockStyle.Fill;

            xf.Controls.Add(cc);
            xf.FormBorderStyle = FormBorderStyle.None;
            xf.StartPosition = FormStartPosition.CenterScreen;


        }          

        private string GetMac()
        {
            string Mac = string.Empty;
            ManagementClass MC = new ManagementClass("Win32_NetworkAdapter");
            ManagementObjectCollection MOCol = MC.GetInstances();
            foreach (ManagementObject MO in MOCol)
                if (MO != null)
                {
                    if (MO["MacAddress"] != null)
                    {
                        Mac = MO["MACAddress"].ToString();
                        if (Mac != string.Empty)
                            break;
                    }
                }

            return Mac;
        }

        private static string GetIpAddress()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }

        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        public static Boolean FechaValida(String fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                Image returnImage = Image.FromStream(ms, true);//Exception occurs here
                return returnImage;
            }
            catch 
            { 
                return null; 
            }
        }

        public PrinterStatus StatusErrorIF(Tfhka Impresora)
        {
            try
            {
                return  Impresora.GetPrinterStatus();
            }
            catch
            {
                return null;
            }
        }

        public static string buscar_impresora(string puerto, string serial)
        {
            bool Respuesta;
            Tfhka Impresora = new Tfhka();

            try
            {
                parametros.PuertoImpresoraFiscal = null;

                //Abre el puerto COM configurado en la estacion
                Respuesta = Impresora.OpenFpCtrl(puerto);

                if (Respuesta)
                {
                    Respuesta = Impresora.CheckFPrinter();

                    if (Respuesta)
                    {
                        S1PrinterData DatosImpresora;

                        DatosImpresora = Impresora.GetS1PrinterData();

                        if (DatosImpresora.RegisteredMachineNumber == serial)
                        {
                            parametros.PuertoImpresoraFiscal = puerto;

                            return puerto;
                        }
                    }
                }

                cerrar_puerto_com(Impresora);

                return null;
            }
            catch (Exception Ex)
            {
                cerrar_puerto_com(Impresora);
                throw new Exception(Ex.Message);
            }
        }

        private static void cerrar_puerto_com(Tfhka Impresora)
        {
            try
            {
                Impresora.CloseFpCtrl();
            }
            catch (Exception ex)
            {
            }
        }

        public static DataTable lista_impresoras()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("impresora");

                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    table.Rows.Add(printer);
                };

                return table;
            }
            catch
            {
                return null;
            }
        }

        public static string generar_codigos_aletorios()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[8];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            return new string(Charsarr);
        }

        public static sys_usr_cfg obtener_configuracion_usuario(string codigo_usuario)
        {
                    ServicioSuiteClient svc = new ServicioSuiteClient("BasicHttpBinding_IServicioSuite", parametros.endpoint_address);

            var usrcfg = svc.Usuario_Buscar_Configuracion(parametros.Sucursal.codigo, codigo_usuario);

            #region Guardar configuracion M0

            //Desencriptamos las información           

            if (!string.IsNullOrEmpty(usrcfg.M0))
            {
                usrcfg.M0 = EncryptionHelper.Desencriptar(usrcfg.M0);

                //Separamos por submodulos
                string[] sm_M0 = usrcfg.M0.Split('|');

                if (sm_M0.Length > 0)
                {
                    foreach (string item in sm_M0)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Substring(0, 3).Split('^');

                            sys_submodulos_opciones smo_M1 = new sys_submodulos_opciones()
                            {
                                codigo = opciones[1],
                                acceso = (opciones[0] == "0") ? false : true
                            };

                            parametros.Lista_Opciones_Usuario_M0.Add(smo_M1);
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M00

            //Desencriptamos las información           

            if (!string.IsNullOrEmpty(usrcfg.M0))
            {
                usrcfg.M0 = EncryptionHelper.Desencriptar(usrcfg.M0);

                //Separamos por submodulos
                string[] sm_M0 = usrcfg.M0.Split('|');

                if (sm_M0.Length > 0)
                {
                    foreach (string item in sm_M0)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            //string[] opciones = item.Substring(3, item.Length).Split('^');

                            string idmodulo = item.Substring(2, 1);

                            string[] opciones = item.Substring(3).Split('~');

                            foreach (string item2 in opciones)
                            {
                                if (!string.IsNullOrEmpty(item2))
                                {
                                    var t = item2.Substring(item2.Length - 1, 1);

                                    sys_submodulos_opciones smo_M1 = new sys_submodulos_opciones()
                                    {
                                        codigo = idmodulo.ToString(),
                                        codigo_submodulo = item2.Substring(1),
                                        acceso = (item2.Substring(0, 1) == "0") ? false : true
                                    };

                                    parametros.Lista_Opciones_Usuario_M00.Add(smo_M1);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M1

            //Desencriptamos las información           

            if (!string.IsNullOrEmpty(usrcfg.M1))
            {
                usrcfg.M1 = EncryptionHelper.Desencriptar(usrcfg.M1);

                //Separamos por submodulos
                string[] sm_M1 = usrcfg.M1.Split('~');

                if (sm_M1.Length > 0)
                {
                    foreach (string item in sm_M1)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo_M1 = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M1.Add(smo_M1);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M2

            //Desencriptamos las información
            
            if (!string.IsNullOrEmpty(usrcfg.M2))
            {
                usrcfg.M2 = EncryptionHelper.Desencriptar(usrcfg.M2);

                //Separamos por submodulos
                string[] sm_M2 = usrcfg.M2.Split('~');

                if (sm_M2.Length > 0)
                {
                    foreach (string item in sm_M2)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M2.Add(smo);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M3

            //Desencriptamos las información            
            
            if (!string.IsNullOrEmpty(usrcfg.M3))
            {
                usrcfg.M3 = EncryptionHelper.Desencriptar(usrcfg.M3);

                //Separamos por submodulos
                string[] sm_M3 = usrcfg.M3.Split('~');

                if (sm_M3.Length > 0)
                {
                    foreach (string item in sm_M3)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M3.Add(smo);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M4

            //Desencriptamos las información
            
            if (!string.IsNullOrEmpty(usrcfg.M4))
            {
                usrcfg.M4 = EncryptionHelper.Desencriptar(usrcfg.M4);

                //Separamos por submodulos
                string[] sm_M4 = usrcfg.M4.Split('~');

                if (sm_M4.Length > 0)
                {
                    foreach (string item in sm_M4)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M4.Add(smo);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M5

            //Desencriptamos las información            

            if (!string.IsNullOrEmpty(usrcfg.M5))
            {
                usrcfg.M5 = EncryptionHelper.Desencriptar(usrcfg.M5);

                //Separamos por submodulos
                string[] sm_M5 = usrcfg.M5.Split('~');

                if (sm_M5.Length > 0)
                {
                    foreach (string item in sm_M5)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M5.Add(smo);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M6

            //Desencriptamos las información
            
            if (!string.IsNullOrEmpty(usrcfg.M6))
            {
                usrcfg.M6 = EncryptionHelper.Desencriptar(usrcfg.M6);

                //Separamos por submodulos
                string[] sm_M6 = usrcfg.M6.Split('~');

                if (sm_M6.Length > 0)
                {
                    foreach (string item in sm_M6)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M6.Add(smo);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Guardar configuracion M7

            //Desencriptamos las información

            if (!string.IsNullOrEmpty(usrcfg.M7))
            {
                usrcfg.M7 = EncryptionHelper.Desencriptar(usrcfg.M7);

                //Separamos por submodulos
                string[] sm_M7 = usrcfg.M7.Split('~');

                if (sm_M7.Length > 0)
                {
                    foreach (string item in sm_M7)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            //Obtenemos los valores de la configuracion de la
                            //lista que se armo al expandir el nodo padre
                            string[] opciones = item.Split('|');

                            foreach (string o in opciones)
                            {
                                //Desglosamos la información y mostramos los valores
                                int start = o.IndexOf("[") + 1;
                                int end = o.IndexOf("]", start);
                                string codigo_opcion = o.Substring(0, start - 1);
                                string result = o.Substring(start, end - start);

                                string[] valores = result.Split('/');

                                sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                {
                                    codigo = codigo_opcion,
                                    valor1 = valores[0],
                                    valor2 = valores[1],
                                };

                                parametros.Lista_Opciones_Usuario_M7.Add(smo);
                            }
                        }
                    }
                }
            }

            #endregion

            return usrcfg;

        }

        public static List<sys_submodulos_opciones> consulta_configuracion_usuario(string codigo_usuario, string modulo)
        {
                    ServicioSuiteClient svc = new ServicioSuiteClient("BasicHttpBinding_IServicioSuite", parametros.endpoint_address);

            var usrcfg = svc.Usuario_Buscar_Configuracion(parametros.Sucursal.codigo, codigo_usuario);

            List<sys_submodulos_opciones> lista_resultado = new List<sys_submodulos_opciones>();

            switch (modulo)
            {
                case "M1":

                    #region Guardar configuracion M1

                    //Desencriptamos las información           

                    if (!string.IsNullOrEmpty(usrcfg.M1))
                    {
                        usrcfg.M1 = EncryptionHelper.Desencriptar(usrcfg.M1);

                        //Separamos por submodulos
                        string[] sm_M1 = usrcfg.M1.Split('~');

                        if (sm_M1.Length > 0)
                        {
                            foreach (string item in sm_M1)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo_M1 = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo_M1);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    break;
                case "M2":

                    #region Guardar configuracion M2

                    //Desencriptamos las información

                    if (!string.IsNullOrEmpty(usrcfg.M2))
                    {
                        usrcfg.M2 = EncryptionHelper.Desencriptar(usrcfg.M2);

                        //Separamos por submodulos
                        string[] sm_M2 = usrcfg.M2.Split('~');

                        if (sm_M2.Length > 0)
                        {
                            foreach (string item in sm_M2)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    break;
                case "M3":

                    #region Guardar configuracion M3

                    //Desencriptamos las información            

                    if (!string.IsNullOrEmpty(usrcfg.M3))
                    {
                        usrcfg.M3 = EncryptionHelper.Desencriptar(usrcfg.M3);

                        //Separamos por submodulos
                        string[] sm_M3 = usrcfg.M3.Split('~');

                        if (sm_M3.Length > 0)
                        {
                            foreach (string item in sm_M3)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    break;
                case "M4":

                    #region Guardar configuracion M4

                    //Desencriptamos las información

                    if (!string.IsNullOrEmpty(usrcfg.M4))
                    {
                        usrcfg.M4 = EncryptionHelper.Desencriptar(usrcfg.M4);

                        //Separamos por submodulos
                        string[] sm_M4 = usrcfg.M4.Split('~');

                        if (sm_M4.Length > 0)
                        {
                            foreach (string item in sm_M4)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    break;
                case "M5":

                    #region Guardar configuracion M5

                    //Desencriptamos las información            

                    if (!string.IsNullOrEmpty(usrcfg.M5))
                    {
                        usrcfg.M5 = EncryptionHelper.Desencriptar(usrcfg.M5);

                        //Separamos por submodulos
                        string[] sm_M5 = usrcfg.M5.Split('~');

                        if (sm_M5.Length > 0)
                        {
                            foreach (string item in sm_M5)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    break;
                case "M6":

                    #region Guardar configuracion M6

                    //Desencriptamos las información

                    if (!string.IsNullOrEmpty(usrcfg.M6))
                    {
                        usrcfg.M6 = EncryptionHelper.Desencriptar(usrcfg.M6);

                        //Separamos por submodulos
                        string[] sm_M6 = usrcfg.M6.Split('~');

                        if (sm_M6.Length > 0)
                        {
                            foreach (string item in sm_M6)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    break;

                case "M7":

                    #region Guardar configuracion M7

                    //Desencriptamos las información

                    if (!string.IsNullOrEmpty(usrcfg.M7))
                    {
                        usrcfg.M7 = EncryptionHelper.Desencriptar(usrcfg.M7);

                        //Separamos por submodulos
                        string[] sm_M7 = usrcfg.M7.Split('~');

                        if (sm_M7.Length > 0)
                        {
                            foreach (string item in sm_M7)
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    //Obtenemos los valores de la configuracion de la
                                    //lista que se armo al expandir el nodo padre
                                    string[] opciones = item.Split('|');

                                    foreach (string o in opciones)
                                    {
                                        //Desglosamos la información y mostramos los valores
                                        int start = o.IndexOf("[") + 1;
                                        int end = o.IndexOf("]", start);
                                        string codigo_opcion = o.Substring(0, start - 1);
                                        string result = o.Substring(start, end - start);

                                        string[] valores = result.Split('/');

                                        sys_submodulos_opciones smo = new sys_submodulos_opciones()
                                        {
                                            codigo = codigo_opcion,
                                            valor1 = valores[0],
                                            valor2 = valores[1],
                                        };

                                        lista_resultado.Add(smo);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    break;
                default:
                    break;
            }

            return lista_resultado;

        }

        public static decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }

    }

    public static class EncryptionHelper
    {
        public static string Encriptar(string texto)
        {
            try
            {

                string key = "qualityinfosolutions"; //llave para encriptar datos

                byte[] keyArray;

                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);

            }
            catch (Exception)
            {

            }
            return texto;
        }

        public static string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "qualityinfosolutions";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception)
            {

            }
            return textoEncriptado;
        }
    }


}
