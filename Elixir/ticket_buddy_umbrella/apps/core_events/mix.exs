defmodule CoreEvents.MixProject do
  use Mix.Project

  def project do
    [
      app: :core_events,
      version: "0.1.0",
      build_path: "../../_build",
      config_path: "../../config/config.exs",
      deps_path: "../../deps",
      lockfile: "../../mix.lock",
      elixir: "~> 1.18",
      start_permanent: Mix.env() == :prod,
      consolidate_protocols: Mix.env() != :dev,
      deps: deps()
    ]
  end

  # Run "mix help compile.app" to learn about applications.
  def application do
    [
      extra_applications: [:logger],
      mod: {CoreEvents.Application, []}
    ]
  end

  # Run "mix help deps" to learn about dependencies.
  defp deps do
    [
      {:usage_rules, "~> 0.1", only: [:dev]},
      {:igniter, "~> 0.5", only: [:dev, :test]},
      {:ecto_sql, "~> 3.13"},
      {:postgrex, ">= 0.0.0"},
      {:shared_telemetry, in_umbrella: true},
      {:ash, "~> 3.5"},
      {:ash_postgres, "~> 2.6"},
      {:spark, "~> 2.2", runtime: false}
    ]
  end
end
