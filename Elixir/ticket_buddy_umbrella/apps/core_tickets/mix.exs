defmodule CoreTickets.MixProject do
  use Mix.Project

  def project do
    [
      app: :core_tickets,
      version: "0.1.0",
      build_path: "../../_build",
      config_path: "../../config/config.exs",
      deps_path: "../../deps",
      lockfile: "../../mix.lock",
      elixir: "~> 1.18",
      start_permanent: Mix.env() == :prod,
      deps: deps()
    ]
  end

  # Run "mix help compile.app" to learn about applications.
  def application do
    [
  extra_applications: [:logger],
  mod: {CoreTickets.Application, []}
    ]
  end

  # Run "mix help deps" to learn about dependencies.
  defp deps do
    [
  {:ecto_sql, "~> 3.11"},
  {:postgrex, ">= 0.0.0"},
  {:shared_telemetry, in_umbrella: true}
    ]
  end
end
