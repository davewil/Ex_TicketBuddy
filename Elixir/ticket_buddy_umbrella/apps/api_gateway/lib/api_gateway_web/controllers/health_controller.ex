defmodule ApiGatewayWeb.HealthController do
  use ApiGatewayWeb, :controller

  def show(conn, _params) do
    json(conn, %{status: "ok"})
  end
end
