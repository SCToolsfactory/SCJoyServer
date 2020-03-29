using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SCJoyServer.VJoy;
using vjMapper.VjOutput;

namespace UT_VJServer2
{
  [TestClass]
  public class UT_VJCmd
  {
    [TestMethod]
    public void T_Translate_01X()
    {
      string test = "{ \"A\": {\"Direction\":\"X\", \"Value\":256}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Axis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_X, ret.CtrlDirection );
      Assert.AreEqual( 256, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_01Y()
    {
      string test = "{ \"A\": {\"Direction\":\"Y\", \"Value\":0}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Axis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Y, ret.CtrlDirection );
      Assert.AreEqual( 0, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_01Z()
    {
      string test = "{ \"A\": {\"Direction\":\"Z\", \"Value\":1000}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Axis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Z, ret.CtrlDirection );
      Assert.AreEqual( 1000, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_01fail()
    {
      string test = "{ \"A\": {\"Direction\":\"Z\", \"Value\":1001}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_02X()
    {
      string test = "{ \"R\": {\"Direction\":\"X\", \"Value\":500}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_RotAxis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_X, ret.CtrlDirection );
      Assert.AreEqual( 500, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_02Y()
    {
      string test = "{ \"R\": {\"Direction\":\"Y\", \"Value\":1000}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_RotAxis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Y, ret.CtrlDirection );
      Assert.AreEqual( 1000, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_02Z()
    {
      string test = "{ \"R\": {\"Direction\":\"Z\", \"Value\":0}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_RotAxis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Z, ret.CtrlDirection );
      Assert.AreEqual( 0, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_02fail()
    {
      string test = "{ \"R\": {\"Direction\":\"P\", \"Value\":0}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_03S1()
    {
      string test = "{ \"S\": {\"Index\":1, \"Value\":1000}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Slider, ret.CtrlType );
      Assert.AreEqual( 1, ret.CtrlIndex );
      Assert.AreEqual( 1000, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_03S2()
    {
      string test = "{ \"S\": {\"Index\":2, \"Value\":0}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Slider, ret.CtrlType );
      Assert.AreEqual( 2, ret.CtrlIndex );
      Assert.AreEqual( 0, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_03fail()
    {
      string test = "{ \"S\": {\"Index\":3, \"Value\":0}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_04c()
    {
      string test = "{ \"P\": {\"Index\":1, \"Direction\":\"c\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Hat, ret.CtrlType );
      Assert.AreEqual( 1, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Center, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_04u()
    {
      string test = "{ \"P\": {\"Index\":2, \"Direction\":\"u\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Hat, ret.CtrlType );
      Assert.AreEqual( 2, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Up, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_04d()
    {
      string test = "{ \"P\": {\"Index\":3, \"Direction\":\"d\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Hat, ret.CtrlType );
      Assert.AreEqual( 3, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Down, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_04l()
    {
      string test = "{ \"P\": {\"Index\":4, \"Direction\":\"l\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Hat, ret.CtrlType );
      Assert.AreEqual( 4, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Left, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_04r()
    {
      string test = "{ \"P\": {\"Index\":1, \"Direction\":\"r\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Hat, ret.CtrlType );
      Assert.AreEqual( 1, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Right, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_04fail()
    {
      string test = "{ \"P\": {\"Index\":1, \"Direction\":\"x\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_05p()
    {
      string test = "{ \"B\": {\"Index\": 31, \"Mode\":\"p\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Button, ret.CtrlType );
      Assert.AreEqual( 31, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Down, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_05r()
    {
      string test = "{ \"B\": {\"Index\": 31, \"Mode\":\"r\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Button, ret.CtrlType );
      Assert.AreEqual( 31, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Up, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_05t()
    {
      string test = "{ \"B\": {\"Index\": 31, \"Mode\":\"t\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Button, ret.CtrlType );
      Assert.AreEqual( 31, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Tap, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_05s()
    {
      string test = "{ \"B\": {\"Index\": 31, \"Mode\":\"s\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Button, ret.CtrlType );
      Assert.AreEqual( 31, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Tap, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_05fail()
    {
      string test = "{ \"B\": {\"Index\": 61, \"Mode\":\"t\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_05fail2()
    {
      string test = "{ \"B\": {\"Index\": 60, \"Mode\":\"x\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_06()
    {
      string test = "{ \"K\": {\"VKcode\": \"123\", \"Mode\":\"r\"}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.DX_Key, ret.CtrlType );
      Assert.AreEqual( 123, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Up, ret.CtrlDirection );
    }

    [TestMethod]
    public void T_Translate_07()
    {
      string test = "{ \"K\": {\"VKcode\": \"123\", \"Mode\":\"t\", \"Delay\": 250}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.DX_Key, ret.CtrlType );
      Assert.AreEqual( 123, ret.CtrlIndex );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Tap, ret.CtrlDirection );
      Assert.AreEqual( 250, ret.CtrlValue );
    }

    [TestMethod]
    public void T_Translate_08fail1()
    {
      string test = "{ \"A\": {\"direction\":\"X\", \"Value\":256}}"; // invalid case od Direction
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsFalse( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Unknown, ret.CtrlType );
    }

    [TestMethod]
    public void T_Translate_09() // multi message
    {
      string test = "{ \"A\": {\"Direction\":\"X\", \"Value\":256}} { \"A\": {\"Direction\":\"Y\", \"Value\":256}}";
      var ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Axis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_X, ret.CtrlDirection );
      Assert.AreEqual( 256, ret.CtrlValue );
      Assert.AreEqual( " { \"A\": {\"Direction\":\"Y\", \"Value\":256}}", test );
      // next message
      ret = VJoyCommand.TranslateMessage( ref test );
      Assert.AreEqual( true, ret != null );
      Assert.IsTrue( ret.IsValid );
      Assert.AreEqual( VJ_ControllerType.VJ_Axis, ret.CtrlType );
      Assert.AreEqual( VJ_ControllerDirection.VJ_Y, ret.CtrlDirection );
      Assert.AreEqual( 256, ret.CtrlValue );
      Assert.AreEqual( "", test );

    }


  }
}
