defmodule TicketBuddyUmbrella.MixProject do
  use Mix.Project

  def project do
    [
      apps_path: "apps",
      version: "0.1.0",
      start_permanent: Mix.env() == :prod,
      consolidate_protocols: Mix.env() != :dev,
      deps: deps(),
      listeners: [Phoenix.CodeReloader]
    ]
  end

  # Dependencies listed here are available only for this
  # project and cannot be accessed from applications inside
  # the apps folder.
  #
  # Run "mix help deps" for examples and options.
  defp deps do
    [
      {:usage_rules, "~> 0.1", only: [:dev]},
      {:igniter, "~> 0.5", only: [:dev, :test]}
    ]
  end
end
