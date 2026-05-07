var Assignment_Filter_ECommerce = (function () {
  function loadControlEcomerce() {
    if ($find(Risk_Assignment_Filter_ECommerce_uxFrom) != undefined) {
      Assignment_Filter_ECommerce.chkECommerce_Changed();
      Assignment_Filter_ECommerce.BindEcommerceType();
    }
    if ($find(Risk_Assignment_Filter_Keyed_uxFrom) != undefined) {
      Assignment_Filter_ECommerce.chkKeyed_Changed();
      Assignment_Filter_ECommerce.BindKeyedType();
    }
    if ($find(Risk_Assignment_Filter_Swiped_uxFrom) != undefined) {
      Assignment_Filter_ECommerce.chkSwiped_Changed();
      Assignment_Filter_ECommerce.BindSwipedType();
    }
  }
  function uxCboECommerceType_OnClientSelectedIndexChanged(sender, args) {
    Assignment_Filter_ECommerce.BindEcommerceType(sender);
    return false;
  }
  function BindEcommerceType() {
    filterTypeValue = $find(Risk_Assignment_Filter_ECommerce_uxcbo).get_value();

    switch (filterTypeValue) {
      case "Between":
        $get("idAndEcommerce").style.display = "";
        $get("idToEcommerce").style.display = "";
        break;
      default:
        $get("idAndEcommerce").style.display = "none";
        $get("idToEcommerce").style.display = "none";
        break;
    }
    Assignment_Filter_ECommerce.HideErroMessage($find(Risk_Assignment_Filter_ECommerce_uxFrom));
  }
  function chkECommerce_Changed() {
    if ($get(Risk_Assignment_Filter_ECommerce_uxchk).checked == false) {

      $find(Risk_Assignment_Filter_ECommerce_uxFrom).clear();
      $find(Risk_Assignment_Filter_ECommerce_uxTo).clear();

      $find(Risk_Assignment_Filter_ECommerce_uxFrom).disable();
      $find(Risk_Assignment_Filter_ECommerce_uxTo).disable();
      $find(Risk_Assignment_Filter_ECommerce_uxcbo).disable();
      Assignment_Filter_ECommerce.HideErroMessage($find(Risk_Assignment_Filter_ECommerce_uxFrom));
    }
    else {
      $find(Risk_Assignment_Filter_ECommerce_uxFrom).enable();
      $find(Risk_Assignment_Filter_ECommerce_uxTo).enable();
      $find(Risk_Assignment_Filter_ECommerce_uxcbo).enable();

    }
  }
  function uxCboKeyedType_OnClientSelectedIndexChanged(sender, args) {
    Assignment_Filter_ECommerce.BindKeyedType();
    return false;
  }
  function BindKeyedType() {
    filterTypeValue = $find(Risk_Assignment_Filter_Keyed_uxcbo).get_value();

    switch (filterTypeValue) {
      case "Between":
        $get("idAndKeyed").style.display = "";
        $get("idToKeyed").style.display = "";
        break;
      default:
        $get("idAndKeyed").style.display = "none";
        $get("idToKeyed").style.display = "none";
        break;
    }
    Assignment_Filter_ECommerce.HideErroMessage($find(Risk_Assignment_Filter_Keyed_uxFrom));
  }
  function chkKeyed_Changed() {
    if ($get(Risk_Assignment_Filter_Keyed_uxchk).checked == false) {

      $find(Risk_Assignment_Filter_Keyed_uxFrom).clear();
      $find(Risk_Assignment_Filter_Keyed_uxTo).clear();

      $find(Risk_Assignment_Filter_Keyed_uxFrom).disable();
      $find(Risk_Assignment_Filter_Keyed_uxTo).disable();
      $find(Risk_Assignment_Filter_Keyed_uxcbo).disable();
      Assignment_Filter_ECommerce.HideErroMessage($find(Risk_Assignment_Filter_Keyed_uxFrom));
    }
    else {
      $find(Risk_Assignment_Filter_Keyed_uxFrom).enable();
      $find(Risk_Assignment_Filter_Keyed_uxTo).enable();
      $find(Risk_Assignment_Filter_Keyed_uxcbo).enable();

    }
  }
  function uxCboSwipedType_OnClientSelectedIndexChanged(sender, args) {
    Assignment_Filter_ECommerce.BindSwipedType();
    return false;
  }
  function BindSwipedType() {
    filterTypeValue = $find(Risk_Assignment_Filter_Swiped_uxcbo).get_value();

    switch (filterTypeValue) {
      case "Between":
        $get("idAndSwiped").style.display = "";
        $get("idToSwiped").style.display = "";
        break;
      default:
        $get("idAndSwiped").style.display = "none";
        $get("idToSwiped").style.display = "none";
        break;
    }
    Assignment_Filter_ECommerce.HideErroMessage($find(Risk_Assignment_Filter_Swiped_uxFrom));
  }
  function chkSwiped_Changed() {
    if ($get(Risk_Assignment_Filter_Swiped_uxchk).checked == false) {

      $find(Risk_Assignment_Filter_Swiped_uxFrom).clear();
      $find(Risk_Assignment_Filter_Swiped_uxTo).clear();

      $find(Risk_Assignment_Filter_Swiped_uxFrom).disable();
      $find(Risk_Assignment_Filter_Swiped_uxTo).disable();
      $find(Risk_Assignment_Filter_Swiped_uxcbo).disable();
      Assignment_Filter_ECommerce.HideErroMessage($find(Risk_Assignment_Filter_Swiped_uxFrom));
    }
    else {
      $find(Risk_Assignment_Filter_Swiped_uxFrom).enable();
      $find(Risk_Assignment_Filter_Swiped_uxTo).enable();
      $find(Risk_Assignment_Filter_Swiped_uxcbo).enable();

    }
  }
  function ShowErrorMessage(control, message) {
    $("label[for='" + control.get_id() + "']").each(function (item) {
      if ($(this).hasClass("control-label")) {
        $(this).addClass("label-error");
      }
      else {
        $(this).show();
        $(this).html(message);
      }
    });
    control.focus();
  }
  function HideErroMessage(control) {
    if (control !== null) {
      $("label[for='" + control.get_id() + "']").each(function (item) {
        if ($(this).hasClass("control-label")) {
          $(this).removeClass("label-error");
        }
        else {
          $(this).hide();
        }
      });
    }
  }
  return {
    loadControlEcomerce: loadControlEcomerce,
    uxCboECommerceType_OnClientSelectedIndexChanged: uxCboECommerceType_OnClientSelectedIndexChanged,
    BindEcommerceType: BindEcommerceType,
    chkECommerce_Changed: chkECommerce_Changed,
    uxCboKeyedType_OnClientSelectedIndexChanged: uxCboKeyedType_OnClientSelectedIndexChanged,
    BindKeyedType: BindKeyedType,
    chkKeyed_Changed: chkKeyed_Changed,
    uxCboSwipedType_OnClientSelectedIndexChanged: uxCboSwipedType_OnClientSelectedIndexChanged,
    BindSwipedType: BindSwipedType,
    chkSwiped_Changed: chkSwiped_Changed,
    ShowErrorMessage: ShowErrorMessage,
    HideErroMessage: HideErroMessage
  }
})()
function ValidateEcomerceFilter() {
  var result = true;
  //Ecommerce
  if ($find(Risk_Assignment_Filter_ECommerce_uxFrom) != undefined && $get(Risk_Assignment_Filter_ECommerce_uxchk).checked) {
    var ecommerceFrom = $find(Risk_Assignment_Filter_ECommerce_uxFrom);
    var ecommerceTo = $find(Risk_Assignment_Filter_ECommerce_uxTo);
    if (ecommerceFrom.get_value() === "" || ecommerceFrom.get_value() === undefined) {
      result = false;
      Assignment_Filter_ECommerce.ShowErrorMessage(ecommerceFrom, Risk_Assignment_Filter_ECommerce_Required);
    }
    else if (parseInt(ecommerceFrom.get_value()) > 100) {
      result = false;
      Assignment_Filter_ECommerce.ShowErrorMessage(ecommerceFrom, Risk_Assignment_Filter_ECommerce);
    }
    else {
      Assignment_Filter_ECommerce.HideErroMessage(ecommerceFrom);
    }
    if ($get("idAndEcommerce").style.display != "none" && ecommerceFrom.get_value() !== "" && ecommerceFrom.get_value() != undefined) {
      if (ecommerceTo.get_value() === "" || ecommerceTo.get_value() === undefined) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(ecommerceFrom, Risk_Assignment_Filter_ECommerce_Required);
      }
      else if (parseInt(ecommerceTo.get_value()) > 100) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(ecommerceFrom, Risk_Assignment_Filter_ECommerce);
      }
      else if (parseInt(ecommerceFrom.get_value()) >= parseInt(ecommerceTo.get_value())) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(ecommerceFrom, Risk_Assignment_Filter_ECommerce_To_Greater);
      }
      else {
        Assignment_Filter_ECommerce.HideErroMessage(ecommerceFrom);
      }
    }
  }
  //Keyed
  if ($find(Risk_Assignment_Filter_Keyed_uxFrom) != undefined && $get(Risk_Assignment_Filter_Keyed_uxchk).checked) {
    var KeyedFrom = $find(Risk_Assignment_Filter_Keyed_uxFrom);
    var KeyedTo = $find(Risk_Assignment_Filter_Keyed_uxTo);
    if (KeyedFrom.get_value() === "" || KeyedFrom.get_value() === undefined) {
      result = false;
      Assignment_Filter_ECommerce.ShowErrorMessage(KeyedFrom, Risk_Assignment_Filter_ECommerce_Required);
    }
    else if (parseInt(KeyedFrom.get_value()) > 100) {
      result = false;
      Assignment_Filter_ECommerce.ShowErrorMessage(KeyedFrom, Risk_Assignment_Filter_ECommerce);
    }
    else {
      Assignment_Filter_ECommerce.HideErroMessage(KeyedFrom);
    }
    if ($get("idAndKeyed").style.display != "none" && KeyedFrom.get_value() !== "" && KeyedFrom.get_value() != undefined) {
      if (KeyedTo.get_value() === "" || KeyedTo.get_value() === undefined) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(KeyedFrom, Risk_Assignment_Filter_ECommerce_Required);
      }
      else if (parseInt(KeyedTo.get_value()) > 100) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(KeyedFrom, Risk_Assignment_Filter_ECommerce);
      }
      else if (parseInt(KeyedFrom.get_value()) >= parseInt(KeyedTo.get_value())) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(KeyedFrom, Risk_Assignment_Filter_ECommerce_To_Greater);
      }
      else {
        Assignment_Filter_ECommerce.HideErroMessage(KeyedFrom);
      }
    }
  }
  //Swiped
  if ($find(Risk_Assignment_Filter_Swiped_uxFrom) != undefined && $get(Risk_Assignment_Filter_Swiped_uxchk).checked) {
    var SwipedFrom = $find(Risk_Assignment_Filter_Swiped_uxFrom);
    var SwipedTo = $find(Risk_Assignment_Filter_Swiped_uxTo);
    if (SwipedFrom.get_value() === "" || SwipedFrom.get_value() === undefined) {
      result = false;
      Assignment_Filter_ECommerce.ShowErrorMessage(SwipedFrom, Risk_Assignment_Filter_ECommerce_Required);
    }
    else if (parseInt(SwipedFrom.get_value()) > 100) {
      result = false;
      Assignment_Filter_ECommerce.ShowErrorMessage(SwipedFrom, Risk_Assignment_Filter_ECommerce);
    }
    else {
      Assignment_Filter_ECommerce.HideErroMessage(SwipedFrom);
    }
    if ($get("idAndSwiped").style.display != "none" && SwipedFrom.get_value() !== "" && SwipedFrom.get_value() != undefined) {
      if (SwipedTo.get_value() === "" || SwipedTo.get_value() === undefined) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(SwipedFrom, Risk_Assignment_Filter_ECommerce_Required);
      }
      else if (parseInt(SwipedTo.get_value()) > 100) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(SwipedFrom, Risk_Assignment_Filter_ECommerce);
      }
      else if (parseInt(SwipedFrom.get_value()) >= parseInt(SwipedTo.get_value())) {
        result = false;
        Assignment_Filter_ECommerce.ShowErrorMessage(SwipedFrom, Risk_Assignment_Filter_ECommerce_To_Greater);
      }
      else {
        Assignment_Filter_ECommerce.HideErroMessage(SwipedFrom);
      }
    }
  }
  return result;
}

function ValidateThreeNewFilters() {
  var result = 'none';
  var cbEcommerce = $get(Risk_Assignment_Filter_ECommerce_uxchk);
  var cbKeyed = $get(Risk_Assignment_Filter_Keyed_uxchk);
  var cbSwiped = $get(Risk_Assignment_Filter_Swiped_uxchk);


  if ((cbEcommerce != undefined && cbEcommerce.checked) || (cbKeyed != undefined && cbKeyed.checked) ||
    (cbSwiped != undefined && cbSwiped.checked)) {
    result = 'valid';
  }
  return result;
}
