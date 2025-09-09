defmodule ApiGatewayWeb.ErrorJSONTest do
  use ApiGatewayWeb.ConnCase

  test "renders 404" do
    assert ApiGatewayWeb.ErrorJSON.render("404.json", %{}) == %{errors: %{detail: "Not Found"}}
  end

  test "renders 500" do
    assert ApiGatewayWeb.ErrorJSON.render("500.json", %{}) ==
             %{errors: %{detail: "Internal Server Error"}}
  end
end
