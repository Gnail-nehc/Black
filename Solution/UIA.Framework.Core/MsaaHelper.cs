using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UIA.Framework.Core
{
	public class MsaaHelper
	{
		private const int OBJID_CLIENT = -4;
		private const int CHILDID_SELF = 0;

		public enum AccRole
		{
			ROLE_SYSTEM_TITLEBAR = 0x1,

			ROLE_SYSTEM_MENUBAR = 0x2,

			ROLE_SYSTEM_SCROLLBAR = 0x3,

			ROLE_SYSTEM_GRIP = 0x4,

			ROLE_SYSTEM_SOUND = 0x5,

			ROLE_SYSTEM_CURSOR = 0x6,

			ROLE_SYSTEM_CARET = 0x7,

			ROLE_SYSTEM_ALERT = 0x8,

			ROLE_SYSTEM_WINDOW = 0x9,

			ROLE_SYSTEM_CLIENT = 0xa,

			ROLE_SYSTEM_MENUPOPUP = 0xb,

			ROLE_SYSTEM_MENUITEM = 0xc,

			ROLE_SYSTEM_TOOLTIP = 0xd,

			ROLE_SYSTEM_APPLICATION = 0xe,

			ROLE_SYSTEM_DOCUMENT = 0xf,

			ROLE_SYSTEM_PANE = 0x10,

			ROLE_SYSTEM_CHART = 0x11,

			ROLE_SYSTEM_DIALOG = 0x12,

			ROLE_SYSTEM_BORDER = 0x13,

			ROLE_SYSTEM_GROUPING = 0x14,

			ROLE_SYSTEM_SEPARATOR = 0x15,

			ROLE_SYSTEM_TOOLBAR = 0x16,

			ROLE_SYSTEM_STATUSBAR = 0x17,

			ROLE_SYSTEM_TABLE = 0x18,

			ROLE_SYSTEM_COLUMNHEADER = 0x19,

			ROLE_SYSTEM_ROWHEADER = 0x1a,

			ROLE_SYSTEM_COLUMN = 0x1b,

			ROLE_SYSTEM_ROW = 0x1c,

			ROLE_SYSTEM_CELL = 0x1d,

			ROLE_SYSTEM_LINK = 0x1e,

			ROLE_SYSTEM_HELPBALLOON = 0x1f,

			ROLE_SYSTEM_CHARACTER = 0x20,

			ROLE_SYSTEM_LIST = 0x21,

			ROLE_SYSTEM_LISTITEM = 0x22,

			ROLE_SYSTEM_OUTLINE = 0x23,

			ROLE_SYSTEM_OUTLINEITEM = 0x24,

			ROLE_SYSTEM_PAGETAB = 0x25,

			ROLE_SYSTEM_PROPERTYPAGE = 0x26,

			ROLE_SYSTEM_INDICATOR = 0x27,

			ROLE_SYSTEM_GRAPHIC = 0x28,

			ROLE_SYSTEM_STATICTEXT = 0x29,

			ROLE_SYSTEM_TEXT = 0x2a,

			ROLE_SYSTEM_PUSHBUTTON = 0x2b,

			ROLE_SYSTEM_CHECKBUTTON = 0x2c,

			ROLE_SYSTEM_RADIOBUTTON = 0x2d,

			ROLE_SYSTEM_COMBOBOX = 0x2e,

			ROLE_SYSTEM_DROPLIST = 0x2f,

			ROLE_SYSTEM_PROGRESSBAR = 0x30,

			ROLE_SYSTEM_DIAL = 0x31,

			ROLE_SYSTEM_HOTKEYFIELD = 0x32,

			ROLE_SYSTEM_SLIDER = 0x33,

			ROLE_SYSTEM_SPINBUTTON = 0x34,

			ROLE_SYSTEM_DIAGRAM = 0x35,

			ROLE_SYSTEM_ANIMATION = 0x36,

			ROLE_SYSTEM_EQUATION = 0x37,

			ROLE_SYSTEM_BUTTONDROPDOWN = 0x38,

			ROLE_SYSTEM_BUTTONMENU = 0x39,

			ROLE_SYSTEM_BUTTONDROPDOWNGRID = 0x3a,

			ROLE_SYSTEM_WHITESPACE = 0x3b,

			ROLE_SYSTEM_PAGETABLIST = 0x3c,

			ROLE_SYSTEM_CLOCK = 0x3d,

			ROLE_SYSTEM_SPLITBUTTON = 0x3e,

			ROLE_SYSTEM_IPADDRESS = 0x3f,

			ROLE_SYSTEM_OUTLINEBUTTON = 0x40,
		}


		[DllImport("oleacc.dll")]
		private static extern int AccessibleObjectFromWindow(
			IntPtr hwnd,
			int id,
			ref Guid iid,
			ref IAccessible ppvObject);

		[DllImport("Oleacc.dll")]
		private static extern int AccessibleChildren(
			IAccessible paccContainer,
			int iChildStart,
			int cChildren,
			[Out] object[] rgvarChildren,
			out int pcObtained);

		public static IAccessible GetWindowAccessibleByHwnd(IntPtr hwnd)
		{
			Guid guidCOM = new Guid(0x618736E0, 0x3C3D, 0x11CF, 0x81, 0xC, 0x0, 0xAA, 0x0, 0x38, 0x9B, 0x71);
			IAccessible targetAcc = null;
			int result = AccessibleObjectFromWindow(hwnd, OBJID_CLIENT, ref guidCOM, ref targetAcc);
			//return (IAccessible)targetAcc.accParent;
			return targetAcc;
		}

		public static IAccessible GetAccessibleObjectByNameAndRole(IAccessible iaccWindow, string accName, AccRole accRole)
		{
			IAccessible objReturn = default(IAccessible);
			int childCount = iaccWindow.accChildCount;
			object[] windowChildren = new object[childCount];
			int pcObtained;
			int result = AccessibleChildren(iaccWindow, 0, childCount, windowChildren, out pcObtained);
			string name = "";
			int role;
			for (int i = 0; i < windowChildren.Count(); i++)
			{
				if (windowChildren[i].GetType() != typeof(int) && windowChildren[i].GetType() != typeof(string))
				{
					IAccessible child = windowChildren[i] as IAccessible;
					role = (int)child.get_accRole(CHILDID_SELF);
					name = child.get_accName(CHILDID_SELF);
					if (accName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && role == accRole.GetHashCode())
					{
						return child;
					}
					objReturn = GetAccessibleObjectByNameAndRole(child, accName, accRole);
					if (objReturn != default(IAccessible))
					{
						return objReturn;
					}
				}
			}
			return objReturn;
		}



	}
}
