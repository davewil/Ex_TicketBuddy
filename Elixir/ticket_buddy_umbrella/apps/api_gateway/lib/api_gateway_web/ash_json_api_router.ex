defmodule ApiGatewayWeb.AshJsonApiRouter do
  use AshJsonApi.Router,
    domains: [
      CoreEvents.Domain,
      CoreUsers.Domain,
      CoreTickets.Domain
    ]
end
