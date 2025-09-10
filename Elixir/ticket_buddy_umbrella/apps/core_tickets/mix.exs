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
      elixirc_paths: elixirc_paths(Mix.env()),
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

  # Specifies which paths to compile per environment.
  defp elixirc_paths(:test), do: ["lib", "test/support"]
  defp elixirc_paths(_), do: ["lib"]

  # Run "mix help deps" to learn about dependencies.
  defp deps do
    [
      {:ecto_sql, "~> 3.11"},
      {:postgrex, ">= 0.0.0"},
      {:shared_telemetry, in_umbrella: true},
  {:core_users, in_umbrella: true},
  {:core_events, in_umbrella: true},
      {:ash, "~> 3.5"},
      {:ash_postgres, "~> 2.6"},
      {:spark, "~> 2.2", runtime: false},
      {:ash_json_api, "~> 1.4"},
      {:picosat_elixir, "~> 0.2"},
      {:ash_oban, "~> 0.4.12"}
    ]
  end
end
