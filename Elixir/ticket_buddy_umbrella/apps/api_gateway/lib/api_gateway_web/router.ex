defmodule ApiGatewayWeb.Router do
  use ApiGatewayWeb, :router

  pipeline :api do
  plug :accepts, ["json", "json-api"]
  end

  scope "/api", ApiGatewayWeb do
    pipe_through :api
  get "/health", HealthController, :show

    # Mount AshJsonApi routes
    forward "/", AshJsonApiRouter
  end

  # Enable LiveDashboard and Swoosh mailbox preview in development
  if Application.compile_env(:api_gateway, :dev_routes) do
    # If you want to use the LiveDashboard in production, you should put
    # it behind authentication and allow only admins to access it.
    # If your application does not have an admins-only section yet,
    # you can use Plug.BasicAuth to set up some basic authentication
    # as long as you are also using SSL (which you should anyway).
    import Phoenix.LiveDashboard.Router

    scope "/dev" do
      pipe_through [:fetch_session, :protect_from_forgery]

      live_dashboard "/dashboard", metrics: ApiGatewayWeb.Telemetry
      forward "/mailbox", Plug.Swoosh.MailboxPreview
    end
  end
end
