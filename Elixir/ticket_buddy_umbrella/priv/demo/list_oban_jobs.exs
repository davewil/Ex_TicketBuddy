alias Ecto.Adapters.SQL

rows =
  SQL.query!(CoreUsers.Repo,
    "select id, state, queue, worker, scheduled_at, attempted_at, inserted_at, args from oban_jobs order by inserted_at desc limit 20"
  ).rows

IO.puts("Recent oban_jobs (any queue) count=#{length(rows)}:")
for [id, state, queue, worker, sched, attempt, inserted, args] <- rows do
  IO.puts("- id=#{id} state=#{state} queue=#{queue} worker=#{worker} sched=#{inspect(sched)} attempted_at=#{inspect(attempt)} inserted_at=#{inspect(inserted)} args=#{inspect(args)}")
end
