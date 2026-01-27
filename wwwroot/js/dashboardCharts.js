// Chart.js helper functions for Journal Entry Dashboard

function renderMoodPieChart(labels, data) {
    const ctx = document.getElementById('moodPieChart');

    if (!ctx) {
        console.error('Canvas element not found');
        return;
    }

    // Destroy existing chart if it exists
    if (window.moodChart) {
        window.moodChart.destroy();
    }

    // Define colors for different moods
    const moodColors = {
        'happy': '#10b981',
        'excited': '#f59e0b',
        'grateful': '#8b5cf6',
        'calm': '#3b82f6',
        'content': '#06b6d4',
        'sad': '#6366f1',
        'anxious': '#ef4444',
        'angry': '#dc2626',
        'tired': '#64748b',
        'stressed': '#f97316',
        'neutral': '#94a3b8',
        'default': '#6b7280'
    };

    // Generate colors based on mood labels
    const backgroundColors = labels.map(label => {
        const normalizedLabel = label.toLowerCase();
        return moodColors[normalizedLabel] || moodColors['default'];
    });

    // Create the chart
    window.moodChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: backgroundColors,
                borderWidth: 2,
                borderColor: '#ffffff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 15,
                        font: {
                            size: 12,
                            family: "'Inter', sans-serif"
                        },
                        color: '#374151',
                        usePointStyle: true,
                        pointStyle: 'circle'
                    }
                },
                tooltip: {
                    backgroundColor: '#1f2937',
                    titleColor: '#ffffff',
                    bodyColor: '#ffffff',
                    borderColor: '#374151',
                    borderWidth: 1,
                    padding: 12,
                    displayColors: true,
                    callbacks: {
                        label: function (context) {
                            const label = context.label || '';
                            const value = context.parsed || 0;
                            const total = context.dataset.data.reduce((a, b) => a + b, 0);
                            const percentage = ((value / total) * 100).toFixed(1);
                            return `${label}: ${value} (${percentage}%)`;
                        }
                    }
                }
            }
        }
    });
}