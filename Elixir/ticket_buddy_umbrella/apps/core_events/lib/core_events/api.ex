defmodule CoreEvents.Domain do
  use Ash.Domain

  resources do
    resource CoreEvents.EventResource
  end
end
