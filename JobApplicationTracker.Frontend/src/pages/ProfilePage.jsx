function ProfilePage({ profile }) {
    return (
        <section className="page">
            <div className="page-header">
                <div>
                    <h1>Profile</h1>
                    <p>Store the candidate profile, CV, and skills here.</p>
                </div>
            </div>

            <div className="profile-layout">
                <article className="card profile-card profile-card-primary">
                    <div className="profile-avatar">AM</div>
                    <div>
                        <h2>{profile.name}</h2>
                        <p className="profile-role">{profile.role}</p>
                    </div>

                    <div className="profile-meta-grid">
                        <div className="profile-meta-item">
                            <span>Email</span>
                            <strong>{profile.email}</strong>
                        </div>
                        <div className="profile-meta-item">
                            <span>Location</span>
                            <strong>{profile.location}</strong>
                        </div>
                        <div className="profile-meta-item">
                            <span>CV</span>
                            <strong>{profile.cv}</strong>
                        </div>
                    </div>
                </article>

                <article className="card profile-card profile-card-secondary">
                    <h2>Skills</h2>
                    <div className="tag-list">
                        {profile.skills.map((skill) => (
                            <span key={skill} className="tag">
                    {skill}
                  </span>
                        ))}
                    </div>
                </article>
            </div>
        </section>
    )
}

export default ProfilePage