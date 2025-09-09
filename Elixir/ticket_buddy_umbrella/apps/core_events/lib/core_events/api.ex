defmodule CoreEvents.Domain do
  use Ash.Domain,
    extensions: [AshJsonApi.Domain]

  resources do
    resource CoreEvents.EventResource
  end
end
