# API Testing Script for TicketBuddy Ash Framework

Write-Host "🚀 Testing TicketBuddy Ash Framework API" -ForegroundColor Green
Write-Host ""

$baseUrl = "http://localhost:4000/api"
$headers = @{
    "Accept" = "application/vnd.api+json"
    "Content-Type" = "application/vnd.api+json"
}

# Test Users endpoint
Write-Host "📋 Testing Users API..." -ForegroundColor Yellow
try {
    $usersResponse = Invoke-RestMethod -Uri "$baseUrl/users" -Method GET -Headers $headers
    Write-Host "✅ Users endpoint successful" -ForegroundColor Green
    Write-Host "   Found $($usersResponse.data.Count) users" -ForegroundColor White
    $usersResponse.data | ForEach-Object {
        Write-Host "   - ID: $($_.id), Name: $($_.attributes.name), Email: $($_.attributes.email)" -ForegroundColor White
    }
}
catch {
    Write-Host "❌ Users endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test Events endpoint
Write-Host "🎟️ Testing Events API..." -ForegroundColor Yellow
try {
    $eventsResponse = Invoke-RestMethod -Uri "$baseUrl/events" -Method GET -Headers $headers
    Write-Host "✅ Events endpoint successful" -ForegroundColor Green
    Write-Host "   Found $($eventsResponse.data.Count) events" -ForegroundColor White
    $eventsResponse.data | ForEach-Object {
        Write-Host "   - ID: $($_.id), Name: $($_.attributes.name), Venue: $($_.attributes.venue)" -ForegroundColor White
    }
}
catch {
    Write-Host "❌ Events endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test Tickets endpoint
Write-Host "🎫 Testing Tickets API..." -ForegroundColor Yellow
try {
    $ticketsResponse = Invoke-RestMethod -Uri "$baseUrl/tickets" -Method GET -Headers $headers
    Write-Host "✅ Tickets endpoint successful" -ForegroundColor Green
    Write-Host "   Found $($ticketsResponse.data.Count) tickets" -ForegroundColor White
    $ticketsResponse.data | ForEach-Object {
        Write-Host "   - ID: $($_.id), User: $($_.attributes.user_id), Event: $($_.attributes.event_id), Price: $($_.attributes.price_cents/100), Status: $($_.attributes.status)" -ForegroundColor White
    }
}
catch {
    Write-Host "❌ Tickets endpoint failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎉 API Testing Complete!" -ForegroundColor Green
