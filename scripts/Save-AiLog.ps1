param (
    [Parameter(Mandatory=$true)]
    [string]$ChatId,

    [Parameter(Mandatory=$false)]
    [string]$ConversationBase64,

    [Parameter(Mandatory=$false)]
    [string]$SummaryBase64
)

$ErrorActionPreference = "Stop"

Write-Host "Starting log save for ChatId: $ChatId"

# Get current date for path structure
$date = Get-Date
$year = $date.ToString("yyyy")
$month = $date.ToString("MM")
$day = $date.ToString("dd")

# Define target directory
$logDir = Join-Path "ai-logs" $year | Join-Path -ChildPath $month | Join-Path -ChildPath $day | Join-Path -ChildPath $ChatId
Write-Host "Target directory: $logDir"

# Create directory if it doesn't exist
if (-not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
    Write-Host "Created directory: $logDir"
}

# Process Conversation Log
$conversationPath = Join-Path $logDir "conversation.md"
if (-not [string]::IsNullOrEmpty($ConversationBase64)) {
    try {
        Write-Host "Processing conversation log... (Base64 length: $($ConversationBase64.Length))"
        $bytes = [System.Convert]::FromBase64String($ConversationBase64)
        [System.IO.File]::WriteAllBytes($conversationPath, $bytes)
        Write-Host "Saved conversation log to: $conversationPath"
    }
    catch {
        Write-Error "Failed to save conversation log: $_"
    }
} else {
    Write-Warning "ConversationBase64 was empty. Skipping conversation.md"
}

# Process Summary Log
$summaryPath = Join-Path $logDir "summary.md"
if (-not [string]::IsNullOrEmpty($SummaryBase64)) {
    try {
        Write-Host "Processing summary log... (Base64 length: $($SummaryBase64.Length))"
        $bytes = [System.Convert]::FromBase64String($SummaryBase64)
        [System.IO.File]::WriteAllBytes($summaryPath, $bytes)
        Write-Host "Saved summary log to: $summaryPath"
    }
    catch {
        Write-Error "Failed to save summary log: $_"
    }
} else {
    Write-Warning "SummaryBase64 was empty. Skipping summary.md"
}
