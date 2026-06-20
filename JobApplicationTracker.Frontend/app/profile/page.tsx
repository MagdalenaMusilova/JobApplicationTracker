'use client';

import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { profileService } from '@/services/profile-service';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { Skeleton } from '@/components/ui/skeleton';
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Badge } from '@/components/ui/badge';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  User,
  Mail,
  Calendar,
  Lock,
  Briefcase,
  GraduationCap,
  Award,
  Code,
  Pencil,
  Plus,
  Trash2,
  Save,
  X,
  Upload,
  FileText,
} from 'lucide-react';
import { toast } from 'sonner';
import { format } from 'date-fns';
import type { ChangePasswordDto } from '@/types/User/ChangePasswordDto';
import { AddWorkExperienceModal } from '@/components/profile/add-work-experience-modal';
import { AddEducationModal } from '@/components/profile/add-education-modal';
import { AddTrainingModal } from '@/components/profile/add-training-modal';
import { AddSkillModal } from '@/components/profile/add-skill-modal';

export default function ProfilePage() {
  const [isChangingPassword, setIsChangingPassword] = useState(false);
  const [isEditingEmail, setIsEditingEmail] = useState(false);
  const [isEditingUsername, setIsEditingUsername] = useState(false);
  const [isClearingResume, setIsClearingResume] = useState(false);
  const [isUploadingPdf, setIsUploadingPdf] = useState(false);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [newEmail, setNewEmail] = useState('');
  const [newUsername, setNewUsername] = useState('');
  const [passwordData, setPasswordData] = useState<ChangePasswordDto>({
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  });

  // Modal states for adding resume items
  const [isAddingWorkExperience, setIsAddingWorkExperience] = useState(false);
  const [isAddingEducation, setIsAddingEducation] = useState(false);
  const [isAddingTraining, setIsAddingTraining] = useState(false);
  const [isAddingSkill, setIsAddingSkill] = useState(false);

  const queryClient = useQueryClient();

  const { data: account, isLoading: isLoadingAccount } = useQuery({
    queryKey: ['account'],
    queryFn: () => profileService.getAccount(),
  });

  const { data: resume, isLoading: isLoadingResume } = useQuery({
    queryKey: ['resume'],
    queryFn: async () => {
      try {
        return await profileService.getResume();
      } catch (error: any) {
        // If resume doesn't exist (404), return null instead of throwing
        if (error?.response?.status === 404) {
          return null;
        }
        throw error;
      }
    },
  });

  const isLoading = isLoadingAccount || isLoadingResume;

  const changePasswordMutation = useMutation({
    mutationFn: (data: ChangePasswordDto) => profileService.changePassword(data),
    onSuccess: () => {
      toast.success('Password changed successfully');
      setIsChangingPassword(false);
      setPasswordData({ currentPassword: '', newPassword: '', confirmNewPassword: '' });
    },
    onError: (error: Error) => {
      toast.error(error.message || 'Failed to change password');
    },
  });

  const updateEmailMutation = useMutation({
    mutationFn: (email: string) => profileService.updateEmail(email),
    onSuccess: () => {
      toast.success('Email updated successfully');
      queryClient.invalidateQueries({ queryKey: ['account'] });
      setIsEditingEmail(false);
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.message || 'Failed to update email';
      toast.error(errorMessage);
    },
  });

  const updateUsernameMutation = useMutation({
    mutationFn: (username: string) => profileService.updateUsername(username),
    onSuccess: () => {
      toast.success('Username updated successfully');
      queryClient.invalidateQueries({ queryKey: ['account'] });
      setIsEditingUsername(false);
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to update username';
      toast.error(errorMessage);
    },
  });

  const deleteResumeMutation = useMutation({
    mutationFn: (resumeId: string) => profileService.deleteResume(resumeId),
    onSuccess: () => {
      toast.success('Resume data cleared successfully');
      queryClient.invalidateQueries({ queryKey: ['resume'] });
      setIsClearingResume(false);
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to clear resume data';
      toast.error(errorMessage);
    },
  });

  const extractPdfMutation = useMutation({
    mutationFn: (file: File) => profileService.extractFromPdf(file),
    onSuccess: () => {
      toast.success('Resume data extracted successfully from PDF');
      queryClient.invalidateQueries({ queryKey: ['resume'] });
      setIsUploadingPdf(false);
      setSelectedFile(null);
    },
    onError: (error: Error) => {
      toast.error(error.message || 'Failed to extract resume data from PDF');
    },
  });

  const handleChangePassword = (e: React.FormEvent) => {
    e.preventDefault();
    if (passwordData.newPassword !== passwordData.confirmNewPassword) {
      toast.error('New passwords do not match');
      return;
    }
    changePasswordMutation.mutate(passwordData);
  };

  const handleClearResume = () => {
    if (resume?.id) {
      deleteResumeMutation.mutate(resume.id);
    }
  };

  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      if (file.type !== 'application/pdf') {
        toast.error('Only PDF files are allowed');
        return;
      }
      setSelectedFile(file);
    }
  };

  const handleUploadPdf = () => {
    if (selectedFile) {
      extractPdfMutation.mutate(selectedFile);
    }
  };

  return (
    <ProtectedLayout>
      <div className="space-y-8">
        {/* Header */}
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold tracking-tight text-foreground">Profile</h1>
            <p className="text-muted-foreground mt-1">
              Manage your account and resume information
            </p>
          </div>
          <div className="flex gap-2">
            <Button
              variant="outline"
              onClick={() => setIsUploadingPdf(true)}
            >
              <Upload className="h-4 w-4 mr-2" />
              Upload PDF Resume
            </Button>
            {resume && (
              <Button
                variant="destructive"
                onClick={() => setIsClearingResume(true)}
              >
                <Trash2 className="h-4 w-4 mr-2" />
                Clear All Resume Data
              </Button>
            )}
          </div>
        </div>

        {isLoading ? (
          <div className="space-y-6">
            <Skeleton className="h-[200px] w-full" />
            <Skeleton className="h-[300px] w-full" />
            <Skeleton className="h-[200px] w-full" />
          </div>
        ) : account ? (
          <div className="space-y-8">
            {/* Account Section */}
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <User className="h-5 w-5" />
                  Account Information
                </CardTitle>
                <CardDescription>
                  Your account details and security settings
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                {/* Email */}
                <div className="flex items-center justify-between py-3 border-b border-border">
                  <div className="flex items-center gap-3">
                    <div className="p-2 rounded-lg bg-muted">
                      <Mail className="h-4 w-4 text-muted-foreground" />
                    </div>
                    <div>
                      <p className="text-sm text-muted-foreground">Email</p>
                      {isEditingEmail ? (
                        <div className="flex items-center gap-2 mt-1">
                          <Input
                            type="email"
                            value={newEmail}
                            onChange={(e) => setNewEmail(e.target.value)}
                            className="h-8 w-64"
                          />
                          <Button
                            size="sm"
                            onClick={() => updateEmailMutation.mutate(newEmail)}
                            disabled={updateEmailMutation.isPending}
                          >
                            <Save className="h-3 w-3" />
                          </Button>
                          <Button
                            size="sm"
                            variant="ghost"
                            onClick={() => setIsEditingEmail(false)}
                          >
                            <X className="h-3 w-3" />
                          </Button>
                        </div>
                      ) : (
                        <p className="font-medium text-foreground">{account.email}</p>
                      )}
                    </div>
                  </div>
                  {!isEditingEmail && (
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => {
                        setNewEmail(account.email);
                        setIsEditingEmail(true);
                      }}
                    >
                      <Pencil className="h-4 w-4" />
                    </Button>
                  )}
                </div>

                {/* Created At */}
                <div className="flex items-center justify-between py-3 border-b border-border">
                  <div className="flex items-center gap-3">
                    <div className="p-2 rounded-lg bg-muted">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                    </div>
                    <div>
                      <p className="text-sm text-muted-foreground">Member Since</p>
                      <p className="font-medium text-foreground">
                        {format(new Date(account.createdAt), 'MMMM d, yyyy')}
                      </p>
                    </div>
                  </div>
                </div>

                {/* Password */}
                <div className="flex items-center justify-between py-3">
                  <div className="flex items-center gap-3">
                    <div className="p-2 rounded-lg bg-muted">
                      <Lock className="h-4 w-4 text-muted-foreground" />
                    </div>
                    <div>
                      <p className="text-sm text-muted-foreground">Password</p>
                      <p className="font-medium text-foreground">••••••••</p>
                    </div>
                  </div>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => setIsChangingPassword(true)}
                  >
                    Change Password
                  </Button>
                </div>
              </CardContent>
            </Card>

            {/* Work Experience Section */}
            <Card>
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle className="flex items-center gap-2">
                      <Briefcase className="h-5 w-5" />
                      Work Experience
                    </CardTitle>
                    <CardDescription>Your professional work history</CardDescription>
                  </div>
                  <Button size="sm" onClick={() => { console.log('Work Experience Add clicked, resume:', resume); setIsAddingWorkExperience(true); }}>
                    <Plus className="h-4 w-4 mr-2" />
                    Add
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                {resume?.workExperiences && resume.workExperiences.length > 0 ? (
                  <div className="space-y-6">
                    {resume.workExperiences.map((exp, index) => (
                      <div
                        key={exp.id}
                        className={index !== resume.workExperiences.length - 1 ? 'pb-6 border-b border-border' : ''}
                      >
                        <div className="flex items-start justify-between">
                          <div className="space-y-2 flex-1">
                            <div>
                              <h3 className="font-semibold text-foreground">{exp.position}</h3>
                              <p className="text-sm text-muted-foreground">{exp.company}</p>
                              <p className="text-xs text-muted-foreground">
                                {exp.startDate ? format(new Date(exp.startDate), 'MMM yyyy') : 'Unknown'} - {exp.endDate ? format(new Date(exp.endDate), 'MMM yyyy') : 'Present'}
                              </p>
                            </div>
                            {exp.jobDescription.length > 0 && (
                              <ul className="mt-3 space-y-1">
                                {exp.jobDescription.map((desc, i) => (
                                  <li key={i} className="text-sm text-muted-foreground flex items-start gap-2">
                                    <span className="text-primary mt-0.5">•</span>
                                    {desc}
                                  </li>
                                ))}
                              </ul>
                            )}
                            {exp.skills.length > 0 && (
                              <div className="flex flex-wrap gap-1 mt-3">
                                {exp.skills.map((su) => (
                                  <Badge key={su.id} variant="secondary" className="text-xs">
                                    {su.skill.name}
                                  </Badge>
                                ))}
                              </div>
                            )}
                            {exp.notes && (
                              <p className="text-xs text-muted-foreground italic mt-2">{exp.notes}</p>
                            )}
                          </div>
                          <div className="flex gap-1 ml-4">
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <Pencil className="h-4 w-4" />
                            </Button>
                            <Button variant="ghost" size="icon" className="h-8 w-8 text-destructive hover:text-destructive">
                              <Trash2 className="h-4 w-4" />
                            </Button>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <p className="text-center text-muted-foreground py-8">No work experience added yet</p>
                )}
              </CardContent>
            </Card>

            {/* Education Section */}
            <Card>
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle className="flex items-center gap-2">
                      <GraduationCap className="h-5 w-5" />
                      Education
                    </CardTitle>
                    <CardDescription>Your educational background</CardDescription>
                  </div>
                  <Button size="sm" onClick={() => setIsAddingEducation(true)}>
                    <Plus className="h-4 w-4 mr-2" />
                    Add
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                {resume?.education && resume.education.length > 0 ? (
                  <div className="space-y-6">
                    {resume.education.map((edu, index) => (
                      <div
                        key={edu.id}
                        className={index !== resume.education.length - 1 ? 'pb-6 border-b border-border' : ''}
                      >
                        <div className="flex items-start justify-between">
                          <div className="space-y-2 flex-1">
                            <div>
                              <h3 className="font-semibold text-foreground">{edu.degree}</h3>
                              <p className="text-sm text-muted-foreground">{edu.school}</p>
                              {edu.majors.length > 0 && (
                                <p className="text-sm text-muted-foreground">
                                  Major: {edu.majors.join(', ')}
                                </p>
                              )}
                            </div>
                            <Badge variant={edu.isFinished ? 'default' : 'secondary'} className="text-xs">
                              {edu.isFinished ? 'Completed' : 'In Progress'}
                            </Badge>
                            {edu.skills.length > 0 && (
                              <div className="flex flex-wrap gap-1 mt-2">
                                {edu.skills.map((su) => (
                                  <Badge key={su.id} variant="outline" className="text-xs">
                                    {su.skill.name}
                                  </Badge>
                                ))}
                              </div>
                            )}
                            {edu.notes && (
                              <p className="text-xs text-muted-foreground italic mt-2">{edu.notes}</p>
                            )}
                          </div>
                          <div className="flex gap-1 ml-4">
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <Pencil className="h-4 w-4" />
                            </Button>
                            <Button variant="ghost" size="icon" className="h-8 w-8 text-destructive hover:text-destructive">
                              <Trash2 className="h-4 w-4" />
                            </Button>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <p className="text-center text-muted-foreground py-8">No education added yet</p>
                )}
              </CardContent>
            </Card>

            {/* Trainings Section */}
            <Card>
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle className="flex items-center gap-2">
                      <Award className="h-5 w-5" />
                      Trainings & Certifications
                    </CardTitle>
                    <CardDescription>Professional certifications and courses</CardDescription>
                  </div>
                  <Button size="sm" onClick={() => setIsAddingTraining(true)}>
                    <Plus className="h-4 w-4 mr-2" />
                    Add
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                {resume?.trainings && resume.trainings.length > 0 ? (
                  <div className="space-y-6">
                    {resume.trainings.map((training, index) => (
                      <div
                        key={training.id}
                        className={index !== resume.trainings.length - 1 ? 'pb-6 border-b border-border' : ''}
                      >
                        <div className="flex items-start justify-between">
                          <div className="space-y-2 flex-1">
                            <div>
                              <h3 className="font-semibold text-foreground">{training.name}</h3>
                              {training.type && (
                                <Badge variant="outline" className="text-xs mt-1">{training.type}</Badge>
                              )}
                              <p className="text-xs text-muted-foreground mt-1">
                                {training.startDate ? format(new Date(training.startDate), 'MMM yyyy') : ''} 
                                {training.endDate ? ` - ${format(new Date(training.endDate), 'MMM yyyy')}` : ''}
                              </p>
                            </div>
                            {training.certification && training.certification.length > 0 && (
                              <div className="flex flex-wrap gap-1 mt-2">
                                {training.certification.map((cert, i) => (
                                  <Badge key={i} variant="secondary" className="text-xs">
                                    {cert}
                                  </Badge>
                                ))}
                              </div>
                            )}
                            {training.skills.length > 0 && (
                              <div className="flex flex-wrap gap-1 mt-2">
                                {training.skills.map((su) => (
                                  <Badge key={su.id} variant="outline" className="text-xs">
                                    {su.skill.name}
                                  </Badge>
                                ))}
                              </div>
                            )}
                            {training.notes && (
                              <p className="text-xs text-muted-foreground italic mt-2">{training.notes}</p>
                            )}
                          </div>
                          <div className="flex gap-1 ml-4">
                            <Button variant="ghost" size="icon" className="h-8 w-8">
                              <Pencil className="h-4 w-4" />
                            </Button>
                            <Button variant="ghost" size="icon" className="h-8 w-8 text-destructive hover:text-destructive">
                              <Trash2 className="h-4 w-4" />
                            </Button>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <p className="text-center text-muted-foreground py-8">No trainings added yet</p>
                )}
              </CardContent>
            </Card>

            {/* Skills Section */}
            <Card>
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div>
                    <CardTitle className="flex items-center gap-2">
                      <Code className="h-5 w-5" />
                      Skills
                    </CardTitle>
                    <CardDescription>Your technical and professional skills</CardDescription>
                  </div>
                  <Button size="sm" onClick={() => setIsAddingSkill(true)}>
                    <Plus className="h-4 w-4 mr-2" />
                    Add
                  </Button>
                </div>
              </CardHeader>
              <CardContent>
                {resume?.skills && resume.skills.length > 0 ? (
                  <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
                    {resume.skills.map((skill) => (
                      <div
                        key={skill.id}
                        className="p-4 rounded-lg border border-border bg-card hover:bg-muted/50 transition-colors"
                      >
                        <div className="flex items-start justify-between">
                          <div className="space-y-1 flex-1">
                            <h4 className="font-medium text-foreground">{skill.name}</h4>
                            {skill.aliases && skill.aliases.length > 0 && (
                              <p className="text-xs text-muted-foreground">
                                Also: {skill.aliases.join(', ')}
                              </p>
                            )}
                            {skill.notes && (
                              <p className="text-xs text-muted-foreground mt-1">{skill.notes}</p>
                            )}
                          </div>
                          <div className="flex gap-1 ml-2">
                            <Button variant="ghost" size="icon" className="h-7 w-7">
                              <Pencil className="h-3 w-3" />
                            </Button>
                            <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive hover:text-destructive">
                              <Trash2 className="h-3 w-3" />
                            </Button>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <p className="text-center text-muted-foreground py-8">No skills added yet</p>
                )}
              </CardContent>
            </Card>
          </div>
        ) : (
          <Card>
            <CardContent className="py-8 text-center text-muted-foreground">
              Failed to load profile data
            </CardContent>
          </Card>
        )}
      </div>

      {/* Change Password Dialog */}
      <Dialog open={isChangingPassword} onOpenChange={setIsChangingPassword}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Change Password</DialogTitle>
            <DialogDescription>
              Enter your current password and choose a new one
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleChangePassword}>
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="currentPassword">Current Password</Label>
                <Input
                  id="currentPassword"
                  type="password"
                  value={passwordData.currentPassword}
                  onChange={(e) => setPasswordData(prev => ({ ...prev, currentPassword: e.target.value }))}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="newPassword">New Password</Label>
                <Input
                  id="newPassword"
                  type="password"
                  value={passwordData.newPassword}
                  onChange={(e) => setPasswordData(prev => ({ ...prev, newPassword: e.target.value }))}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="confirmNewPassword">Confirm New Password</Label>
                <Input
                  id="confirmNewPassword"
                  type="password"
                  value={passwordData.confirmNewPassword}
                  onChange={(e) => setPasswordData(prev => ({ ...prev, confirmNewPassword: e.target.value }))}
                  required
                />
              </div>
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={() => setIsChangingPassword(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={changePasswordMutation.isPending}>
                {changePasswordMutation.isPending ? 'Changing...' : 'Change Password'}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      {/* Clear Resume Confirmation Dialog */}
      <Dialog open={isClearingResume} onOpenChange={setIsClearingResume}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Clear All Resume Data</DialogTitle>
            <DialogDescription>
              This will permanently delete all your resume information including work experience,
              education, trainings, and skills. This action cannot be undone.
            </DialogDescription>
          </DialogHeader>
          <DialogFooter>
            <Button variant="outline" onClick={() => setIsClearingResume(false)}>
              Cancel
            </Button>
            <Button
              variant="destructive"
              onClick={handleClearResume}
              disabled={deleteResumeMutation.isPending}
            >
              {deleteResumeMutation.isPending ? 'Clearing...' : 'Clear All Data'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Upload PDF Resume Dialog */}
      <Dialog open={isUploadingPdf} onOpenChange={(open) => {
        setIsUploadingPdf(open);
        if (!open) setSelectedFile(null);
      }}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Upload PDF Resume</DialogTitle>
            <DialogDescription>
              Upload your resume in PDF format to automatically extract and populate your profile information.
            </DialogDescription>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div className="border-2 border-dashed border-border rounded-lg p-6">
              <div className="flex flex-col items-center justify-center space-y-3">
                <div className="p-3 rounded-full bg-muted">
                  <FileText className="h-8 w-8 text-muted-foreground" />
                </div>
                <div className="text-center">
                  <Label htmlFor="pdf-upload" className="cursor-pointer">
                    <span className="text-sm font-medium text-primary hover:underline">
                      Click to upload
                    </span>
                    <span className="text-sm text-muted-foreground"> or drag and drop</span>
                  </Label>
                  <p className="text-xs text-muted-foreground mt-1">
                    PDF files only
                  </p>
                </div>
                <Input
                  id="pdf-upload"
                  type="file"
                  accept=".pdf"
                  onChange={handleFileSelect}
                  className="hidden"
                />
                {selectedFile && (
                  <div className="flex items-center gap-2 p-3 bg-muted rounded-md w-full">
                    <FileText className="h-4 w-4 text-muted-foreground" />
                    <span className="text-sm flex-1 truncate">{selectedFile.name}</span>
                    <Button
                      variant="ghost"
                      size="sm"
                      onClick={() => setSelectedFile(null)}
                    >
                      <X className="h-4 w-4" />
                    </Button>
                  </div>
                )}
              </div>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setIsUploadingPdf(false)}>
              Cancel
            </Button>
            <Button
              onClick={handleUploadPdf}
              disabled={!selectedFile || extractPdfMutation.isPending}
            >
              {extractPdfMutation.isPending ? 'Extracting...' : 'Upload & Extract'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Add Resume Item Modals */}
      <AddWorkExperienceModal
        open={isAddingWorkExperience}
        onOpenChange={setIsAddingWorkExperience}
        resumeId={resume?.id || ''}
      />
      <AddEducationModal
        open={isAddingEducation}
        onOpenChange={setIsAddingEducation}
        resumeId={resume?.id || ''}
      />
      <AddTrainingModal
        open={isAddingTraining}
        onOpenChange={setIsAddingTraining}
        resumeId={resume?.id || ''}
      />
      <AddSkillModal
        open={isAddingSkill}
        onOpenChange={setIsAddingSkill}
        resumeId={resume?.id || ''}
      />
    </ProtectedLayout>
  );
}
