// Generates .github/badges/migration-progress.svg based on issues labeled 'migration'
// Uses GitHub REST API via fetch (Node 18+ in GitHub Actions). Auth via GITHUB_TOKEN.

const fs = require('fs');
const path = require('path');

async function fetchAllIssues(owner, repo, token, label) {
  const perPage = 100;
  let page = 1;
  let results = [];
  while (true) {
  const labelsParam = encodeURIComponent(label);
  const url = `https://api.github.com/repos/${owner}/${repo}/issues?state=all&labels=${labelsParam}&per_page=${perPage}&page=${page}`;
    const res = await fetch(url, {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Accept': 'application/vnd.github+json',
        'X-GitHub-Api-Version': '2022-11-28'
      }
    });
    if (!res.ok) {
      const text = await res.text();
      throw new Error(`GitHub API error ${res.status}: ${text}`);
    }
    const batch = await res.json();
    results = results.concat(batch);
    if (batch.length < perPage) break;
    page += 1;
  }
  return results;
}

function escapeXml(s){ return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;'); }

function makeBadge(percent, closed, total) {
  let color = '#e05d44'; // red
  if (percent >= 90) color = '#4c1'; // brightgreen
  else if (percent >= 70) color = '#97CA00'; // yellowgreen
  else if (percent >= 40) color = '#dfb317'; // yellow
  else if (percent >= 20) color = '#fe7d37'; // orange

  const labelText = 'migration';
  const messageText = `${percent}% (${closed}/${total})`;
  const charW = 6; // approximate
  const pad = 20;
  const labelWidth = Math.max(60, labelText.length * charW + pad);
  const messageWidth = Math.max(80, messageText.length * charW + pad);
  const width = labelWidth + messageWidth;

  const svg = [
    '<?xml version="1.0" encoding="UTF-8"?>',
    `<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="20" role="img" aria-label="${escapeXml(labelText)}: ${escapeXml(messageText)}">`,
    '  <linearGradient id="s" x2="0" y2="100%">',
    '    <stop offset="0" stop-color="#bbb" stop-opacity=".1"/>',
    '    <stop offset="1" stop-opacity=".1"/>',
    '  </linearGradient>',
    '  <mask id="m">',
    `    <rect width="${width}" height="20" rx="3" fill="#fff"/>`,
    '  </mask>',
    '  <g mask="url(#m)">',
    `    <rect width="${labelWidth}" height="20" fill="#555"/>`,
    `    <rect x="${labelWidth}" width="${messageWidth}" height="20" fill="${color}"/>`,
    `    <rect width="${width}" height="20" fill="url(#s)"/>`,
    '  </g>',
    '  <g fill="#fff" text-anchor="middle" font-family="Verdana,Geneva,DejaVu Sans,sans-serif" font-size="11">',
    `    <text x="${labelWidth/2}" y="15">${escapeXml(labelText)}</text>`,
    `    <text x="${labelWidth + messageWidth/2}" y="15">${escapeXml(messageText)}</text>`,
    '  </g>',
    '</svg>'
  ].join('\n');
  return svg;
}

(async () => {
  try {
    const token = process.env.GITHUB_TOKEN || process.env.GH_TOKEN || process.env.GITHUB_PAT;
    if (!token) throw new Error('GITHUB_TOKEN not found in environment');
    const repoEnv = process.env.GITHUB_REPOSITORY; // owner/repo in Actions
    if (!repoEnv) throw new Error('GITHUB_REPOSITORY not set');
    const [owner, repo] = repoEnv.split('/');

    const categories = [
      { label: 'migration', file: 'migration-progress.svg', title: 'Overall migration' },
      { label: 'migration: foundations', file: 'migration-foundations.svg', title: 'Foundations' },
      { label: 'migration: dev-env', file: 'migration-dev-env.svg', title: 'Dev Env' },
      { label: 'migration: ash-resources', file: 'migration-ash-resources.svg', title: 'Ash Resources' },
      { label: 'migration: data-migration', file: 'migration-data-migration.svg', title: 'Data Migration' },
      { label: 'migration: api-parity', file: 'migration-api-parity.svg', title: 'API Parity' },
      { label: 'migration: messaging', file: 'migration-messaging.svg', title: 'Messaging' },
      { label: 'migration: observability', file: 'migration-observability.svg', title: 'Observability' },
      { label: 'migration: ci-cd', file: 'migration-ci-cd.svg', title: 'CI/CD' },
      { label: 'migration: cutover', file: 'migration-cutover.svg', title: 'Cutover' },
      { label: 'migration: governance', file: 'migration-governance.svg', title: 'Governance' }
    ];

    const outDir = path.join(process.cwd(), '.github', 'badges');
    fs.mkdirSync(outDir, { recursive: true });

    let anyUpdated = false;

    for (const cat of categories) {
      const issues = await fetchAllIssues(owner, repo, token, cat.label);
      const filtered = issues.filter(i => !i.pull_request);
      const total = filtered.length;
      const closed = filtered.filter(i => i.state === 'closed').length;
      const percent = total ? Math.round((closed / total) * 100) : 0;

      const svg = makeBadge(percent, closed, total);
      const outPath = path.join(outDir, cat.file);
      const prev = fs.existsSync(outPath) ? fs.readFileSync(outPath, 'utf8') : '';
      if (prev !== svg) {
        fs.writeFileSync(outPath, svg, 'utf8');
        anyUpdated = true;
      }
      console.log(`${cat.title}: ${percent}% (${closed}/${total}) -> ${cat.file}`);
    }

    console.log(`UPDATED=${anyUpdated}`);
  } catch (err) {
    console.error(err);
    process.exit(1);
  }
})();
