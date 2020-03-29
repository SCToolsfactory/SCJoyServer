using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SCJoyServer.VJoy;

namespace UT_VJServer2
{
  [TestClass]
  public class UT_Json
  {
    [TestMethod]
    public void T_Extract_01()
    {
      string test = "{ \"key\" : { \"i1\" : 123 } }";
      bool ret = Json.ExtractFragment( test, out string fragment, out string remaining );
      Assert.AreEqual( true, ret );
      Assert.AreEqual( test, fragment );
      Assert.AreEqual( "", remaining );
    }

    [TestMethod]
    public void T_Extract_02()
    {
      string test = "{ \"key\" : { \"i1\" : 123 }";
      bool ret = Json.ExtractFragment( test, out string fragment, out string remaining );
      Assert.AreEqual( false, ret );
      Assert.AreEqual( "", fragment );
      Assert.AreEqual( test, remaining );
    }

    [TestMethod]
    public void T_Extract_03()
    {
      string test = "{ \"key\" : { \"i1\" : 123 } }, { \"keyX\" : { \"i1\" : 123 } }";
      bool ret = Json.ExtractFragment( test, out string fragment, out string remaining );
      Assert.AreEqual( true, ret );
      Assert.AreEqual( "{ \"key\" : { \"i1\" : 123 } }", fragment );
      Assert.AreEqual( ", { \"keyX\" : { \"i1\" : 123 } }", remaining );
    }

  }
}
