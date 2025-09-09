defmodule CoreUsers.Domain do
  use Ash.Domain

  resources do
    resource CoreUsers.UserResource
  end
end
