function StatCard({ kicker, value, label }) {
    return (
        <article className="card stat-card">
            <span className="stat-kicker">{kicker}</span>
            <strong className="stat-value">{value}</strong>
            <span className="stat-label">{label}</span>
        </article>
    )
}

export default StatCard