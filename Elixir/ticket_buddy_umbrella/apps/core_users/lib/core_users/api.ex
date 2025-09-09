defmodule CoreUsers.Domain do
  use Ash.Domain,
    extensions: [AshJsonApi.Domain]

  resources do
    resource CoreUsers.UserResource
  end
end
