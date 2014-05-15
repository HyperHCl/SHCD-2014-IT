using Qisi.Editor.Expression;
using Qisi.Editor.Properties;
using Qisi.General;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text;
namespace Qisi.Editor
{
	internal static class CommonMethods
	{
		internal static int height = 35;
		private static MemoryIniFile ini = null;
		internal static PrivateFontCollection pfc = null;
		internal static float CalcAscentPixel(Font font)
		{
			FontFamily fontFamily = font.FontFamily;
			int cellAscent = fontFamily.GetCellAscent(font.Style);
			return font.Size * (float)cellAscent / (float)fontFamily.GetEmHeight(font.Style);
		}
		internal static string ExprToString(string key)
		{
			if (CommonMethods.ini == null)
			{
				CommonMethods.ini = new MemoryIniFile();
				CommonMethods.ini.LoadFromString(Resources.Expr, Encoding.UTF8);
			}
			return CommonMethods.ini.ReadValue("Expr", key, "");
		}
		internal static string GetSpecialChar(string key)
		{
			if (CommonMethods.ini == null)
			{
				CommonMethods.ini = new MemoryIniFile();
				CommonMethods.ini.LoadFromString(Resources.Expr, Encoding.UTF8);
			}
			return CommonMethods.ini.ReadValue("SpecialChar", key, "");
		}
		internal static string Exprs(string key)
		{
			if (CommonMethods.ini == null)
			{
				CommonMethods.ini = new MemoryIniFile();
				CommonMethods.ini.LoadFromString(Resources.Expr, Encoding.UTF8);
			}
			return CommonMethods.ini.ReadValue("Group", key, "");
		}
		internal static string Groups(string key)
		{
			if (CommonMethods.ini == null)
			{
				CommonMethods.ini = new MemoryIniFile();
				CommonMethods.ini.LoadFromString(Resources.Expr, Encoding.UTF8);
			}
			return CommonMethods.ini.ReadValue("Subject", key, "");
		}
		internal static structexpression CreateExpr(string type, lineexpression parent, Color color, string content = "", int matrixX = 2, int matrixY = 1)
		{
			structexpression result;
			switch (type)
			{
			case "分式":
				result = new fenshi(parent, color);
				return result;
			case "上标":
				result = new shangbiao(parent, color);
				return result;
			case "下标":
				result = new xiabiao(parent, color);
				return result;
			case "斜分式":
				result = new xiefenshi(parent, color);
				return result;
			case "上下标右":
				result = new shangxiabiaoyou(parent, color);
				return result;
			case "上下标左":
				result = new shangxiabiaozuo(parent, color);
				return result;
			case "平方根":
				result = new pingfanggen(parent, color);
				return result;
			case "立方根":
				result = new lifanggen(parent, color);
				return result;
			case "一般根式":
				result = new genshi(parent, color);
				return result;
			case "积分1":
				result = new jifen1(parent, color);
				return result;
			case "积分2":
				result = new jifen2(parent, color);
				return result;
			case "积分3":
				result = new jifen3(parent, color);
				return result;
			case "围道积分1":
				result = new weidao1(parent, color);
				return result;
			case "围道积分2":
				result = new weidao2(parent, color);
				return result;
			case "围道积分3":
				result = new weidao3(parent, color);
				return result;
			case "面积分1":
				result = new mianjifen1(parent, color);
				return result;
			case "面积分2":
				result = new mianjifen2(parent, color);
				return result;
			case "面积分3":
				result = new mianjifen3(parent, color);
				return result;
			case "体积分1":
				result = new tijifen1(parent, color);
				return result;
			case "体积分2":
				result = new tijifen2(parent, color);
				return result;
			case "体积分3":
				result = new tijifen3(parent, color);
				return result;
			case "二重积分1":
				result = new erchongjifen1(parent, color);
				return result;
			case "二重积分2":
				result = new erchongjifen2(parent, color);
				return result;
			case "二重积分3":
				result = new erchongjifen3(parent, color);
				return result;
			case "三重积分1":
				result = new sanchongjifen1(parent, color);
				return result;
			case "三重积分2":
				result = new sanchongjifen2(parent, color);
				return result;
			case "三重积分3":
				result = new sanchongjifen3(parent, color);
				return result;
			case "求和1":
				result = new qiuhe1(parent, color);
				return result;
			case "求和2":
				result = new qiuhe2(parent, color);
				return result;
			case "求和3":
				result = new qiuhe3(parent, color);
				return result;
			case "求和4":
				result = new qiuhe4(parent, color);
				return result;
			case "求和5":
				result = new qiuhe5(parent, color);
				return result;
			case "乘积1":
				result = new chengji1(parent, color);
				return result;
			case "乘积2":
				result = new chengji2(parent, color);
				return result;
			case "乘积3":
				result = new chengji3(parent, color);
				return result;
			case "乘积4":
				result = new chengji4(parent, color);
				return result;
			case "乘积5":
				result = new chengji5(parent, color);
				return result;
			case "副积1":
				result = new fuji1(parent, color);
				return result;
			case "副积2":
				result = new fuji2(parent, color);
				return result;
			case "副积3":
				result = new fuji3(parent, color);
				return result;
			case "副积4":
				result = new fuji4(parent, color);
				return result;
			case "副积5":
				result = new fuji5(parent, color);
				return result;
			case "并11":
				result = new bing11(parent, color);
				return result;
			case "并12":
				result = new bing12(parent, color);
				return result;
			case "并13":
				result = new bing13(parent, color);
				return result;
			case "并14":
				result = new bing14(parent, color);
				return result;
			case "并15":
				result = new bing15(parent, color);
				return result;
			case "交11":
				result = new jiao11(parent, color);
				return result;
			case "交12":
				result = new jiao12(parent, color);
				return result;
			case "交13":
				result = new jiao13(parent, color);
				return result;
			case "交14":
				result = new jiao14(parent, color);
				return result;
			case "交15":
				result = new jiao15(parent, color);
				return result;
			case "并21":
				result = new bing21(parent, color);
				return result;
			case "并22":
				result = new bing22(parent, color);
				return result;
			case "并23":
				result = new bing23(parent, color);
				return result;
			case "并24":
				result = new bing24(parent, color);
				return result;
			case "并25":
				result = new bing25(parent, color);
				return result;
			case "交21":
				result = new jiao21(parent, color);
				return result;
			case "交22":
				result = new jiao22(parent, color);
				return result;
			case "交23":
				result = new jiao23(parent, color);
				return result;
			case "交24":
				result = new jiao24(parent, color);
				return result;
			case "交25":
				result = new jiao25(parent, color);
				return result;
			case "正弦":
				result = new sin(parent, color);
				return result;
			case "余弦":
				result = new cos(parent, color);
				return result;
			case "正切":
				result = new tan(parent, color);
				return result;
			case "余切":
				result = new cot(parent, color);
				return result;
			case "反正弦":
				result = new arcsin(parent, color);
				return result;
			case "反余弦":
				result = new arccos(parent, color);
				return result;
			case "反正切":
				result = new arctan(parent, color);
				return result;
			case "反余切":
				result = new arccot(parent, color);
				return result;
			case "正割":
				result = new sec(parent, color);
				return result;
			case "余割":
				result = new csc(parent, color);
				return result;
			case "双曲正弦":
				result = new sinh(parent, color);
				return result;
			case "双曲余弦":
				result = new cosh(parent, color);
				return result;
			case "双曲正切":
				result = new tanh(parent, color);
				return result;
			case "双曲余切":
				result = new coth(parent, color);
				return result;
			case "双曲正割":
				result = new sech(parent, color);
				return result;
			case "双曲余割":
				result = new csch(parent, color);
				return result;
			case "对数2":
				result = new log(parent, color);
				return result;
			case "对数e":
				result = new ln(parent, color);
				return result;
			case "对数10":
				result = new lg(parent, color);
				return result;
			case "对数":
				result = new logx(parent, color);
				return result;
			case "正弦_2":
				result = new sin_2(parent, color);
				return result;
			case "余弦_2":
				result = new cos_2(parent, color);
				return result;
			case "正切_2":
				result = new tan_2(parent, color);
				return result;
			case "余切_2":
				result = new cot_2(parent, color);
				return result;
			case "反正弦_2":
				result = new arcsin_2(parent, color);
				return result;
			case "反余弦_2":
				result = new arccos_2(parent, color);
				return result;
			case "反正切_2":
				result = new arctan_2(parent, color);
				return result;
			case "反余切_2":
				result = new arccot_2(parent, color);
				return result;
			case "正割_2":
				result = new sec_2(parent, color);
				return result;
			case "余割_2":
				result = new csc_2(parent, color);
				return result;
			case "双曲正弦_2":
				result = new sinh_2(parent, color);
				return result;
			case "双曲余弦_2":
				result = new cosh_2(parent, color);
				return result;
			case "双曲正切_2":
				result = new tanh_2(parent, color);
				return result;
			case "双曲余切_2":
				result = new coth_2(parent, color);
				return result;
			case "双曲正割_2":
				result = new sech_2(parent, color);
				return result;
			case "双曲余割_2":
				result = new csch_2(parent, color);
				return result;
			case "极限":
				result = new lim(parent, color);
				return result;
			case "最大值":
				result = new max(parent, color);
				return result;
			case "最小值":
				result = new min(parent, color);
				return result;
			case "点":
				result = new dian(parent, color);
				return result;
			case "双点":
				result = new shuangdian(parent, color);
				return result;
			case "三点":
				result = new sandian(parent, color);
				return result;
			case "乘幂号":
				result = new chengmihao(parent, color);
				return result;
			case "对号":
				result = new duihao(parent, color);
				return result;
			case "尖音符号":
				result = new jianyinfuhao(parent, color);
				return result;
			case "抑音符号":
				result = new yiyinfuhao(parent, color);
				return result;
			case "短音符号":
				result = new duanyinfuhao(parent, color);
				return result;
			case "颚化符":
				result = new ehuafu(parent, color);
				return result;
			case "横杠":
				result = new henggang(parent, color);
				return result;
			case "双顶线":
				result = new shuangdingxian(parent, color);
				return result;
			case "底线":
				result = new dixian(parent, color);
				return result;
			case "带框公式":
				result = new daikuanggongshi(parent, color);
				return result;
			case "上方大括号":
				result = new shangfangdakuohao(parent, color);
				return result;
			case "下方大括号":
				result = new xiafangdakuohao(parent, color);
				return result;
			case "分组字符在上":
				result = new fenzuzifuzaishang(parent, color);
				return result;
			case "分组字符在下":
				result = new fenzuzifuzaixia(parent, color);
				return result;
			case "左箭头在上":
				result = new zuojiantouzaishang(parent, color);
				return result;
			case "右箭头在上":
				result = new youjiantouzaishang(parent, color);
				return result;
			case "双向箭头在上":
				result = new shuangxiangjiantouzaishang(parent, color);
				return result;
			case "左向简式箭头在上":
				result = new zuoxiangjianshijiantouzaishang(parent, color);
				return result;
			case "右向简式箭头在上":
				result = new youxiangjianshijiantouzaishang(parent, color);
				return result;
			case "大括号":
				result = new dakuohao(parent, color);
				return result;
			case "方括号":
				result = new fangkuohao(parent, color);
				return result;
			case "括号":
				result = new kuohao(parent, color);
				return result;
			case "尖括号":
				result = new jiankuohao(parent, color);
				return result;
			case "单边大括号":
				result = new danbiandakuohao(parent, color);
				return result;
			case "绝对值1":
				result = new jueduizhi1(parent, color);
				return result;
			case "绝对值2":
				result = new jueduizhi2(parent, color);
				return result;
			case "等号1":
				result = new denghao1(parent, color);
				return result;
			case "左箭头1":
				result = new zuojiantou1(parent, color);
				return result;
			case "右箭头1":
				result = new youjiantou1(parent, color);
				return result;
			case "双向箭头1":
				result = new shuangxiangjiantou1(parent, color);
				return result;
			case "等号2":
				result = new denghao2(parent, color);
				return result;
			case "左箭头2":
				result = new zuojiantou2(parent, color);
				return result;
			case "右箭头2":
				result = new youjiantou2(parent, color);
				return result;
			case "双向箭头2":
				result = new shuangxiangjiantou2(parent, color);
				return result;
			case "等号3":
				result = new denghao3(parent, color);
				return result;
			case "左箭头3":
				result = new zuojiantou3(parent, color);
				return result;
			case "右箭头3":
				result = new youjiantou3(parent, color);
				return result;
			case "双向箭头3":
				result = new shuangxiangjiantou3(parent, color);
				return result;
			case "矩阵":
				result = new juzheng(new Point(matrixX, matrixY), parent, color);
				return result;
			case "加减号":
			case "无穷":
			case "不等于":
			case "乘号":
			case "三角":
			case "除号":
			case "成比例":
			case "远小于":
			case "远大于":
			case "小于等于":
			case "大于等于":
			case "减加号":
			case "约等于":
			case "几乎等于":
			case "等价于":
			case "任意":
			case "补集":
			case "偏微分":
			case "平方根号":
			case "立方根号":
			case "四方根号":
			case "并":
			case "交":
			case "空集":
			case "度":
			case "华氏度":
			case "摄氏度":
			case "燃烧":
			case "差值":
			case "递增":
			case "递减":
			case "存在":
			case "不存在":
			case "属于于":
			case "属于":
			case "不属于":
			case "包含":
			case "包含于":
			case "不包含1":
			case "不包含于1":
			case "不包含2":
			case "不包含于2":
			case "左箭头":
			case "上箭头":
			case "下箭头":
			case "右箭头":
			case "双向箭头":
			case "因为":
			case "所以":
			case "未签名":
			case "希腊1":
			case "希腊2":
			case "希腊3":
			case "希腊4":
			case "希腊5":
			case "希腊6":
			case "希腊7":
			case "希腊8":
			case "希腊9":
			case "希腊10":
			case "希腊11":
			case "希腊12":
			case "希腊13":
			case "希腊14":
			case "希腊15":
			case "希腊16":
			case "希腊17":
			case "星乘":
			case "点乘":
			case "着重点":
			case "垂直省略号":
			case "中间水平省略号":
			case "右上对角省略号":
			case "右下对角省略号":
			case "希腊18":
			case "希腊19":
			case "直角":
			case "角度":
			case "测量角":
			case "球面角":
			case "以弧度表示的直角":
			case "直角三角形":
			case "等于且平行于":
			case "垂直于":
			case "不整除":
			case "不平行于":
			case "平行于":
			case "比率":
			case "比例":
			case "左双线箭头":
			case "右双线箭头":
			case "左右双线箭头":
				result = new specialchar(CommonMethods.GetSpecialChar(type), (FType)Enum.Parse(typeof(FType), type, true), CommonMethods.ExprToString(type), parent, color);
				return result;
			}
			result = new charexpression(content, parent, color);
			return result;
		}
		public static Font GetCambriaFont(float fontsize = 12f, FontStyle FS = FontStyle.Regular)
		{
			if (CommonMethods.pfc == null)
			{
				CommonMethods.pfc = new PrivateFontCollection();
				byte[] array = Resources.cambria;
				IntPtr intPtr = Marshal.AllocCoTaskMem(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				CommonMethods.pfc.AddMemoryFont(intPtr, array.Length);
				Marshal.FreeCoTaskMem(intPtr);
				array = Resources.cambriab;
				intPtr = Marshal.AllocCoTaskMem(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				CommonMethods.pfc.AddMemoryFont(intPtr, array.Length);
				Marshal.FreeCoTaskMem(intPtr);
				array = Resources.cambriaz;
				intPtr = Marshal.AllocCoTaskMem(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				CommonMethods.pfc.AddMemoryFont(intPtr, array.Length);
				Marshal.FreeCoTaskMem(intPtr);
				array = Resources.cambriai;
				intPtr = Marshal.AllocCoTaskMem(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				CommonMethods.pfc.AddMemoryFont(intPtr, array.Length);
				Marshal.FreeCoTaskMem(intPtr);
			}
			Font result;
			if (CommonMethods.pfc != null)
			{
				for (int i = 0; i < CommonMethods.pfc.Families.Length; i++)
				{
					if (CommonMethods.pfc.Families[i].Name == "Cambria")
					{
						result = new Font(CommonMethods.pfc.Families[i], fontsize, FS, GraphicsUnit.Pixel);
						return result;
					}
				}
			}
			result = SystemFonts.DefaultFont;
			return result;
		}
	}
}
